using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using POCBankApp;

namespace POCBankApp.Controllers
{
    public class TransactionsController : ApiController
    {
        private EFConnectionString db = new EFConnectionString();

        // GET: api/Transactions
        public IQueryable<Transaction> GetTransactions()
        {
            return db.Transactions;
        }

        // GET: api/Transactions/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult GetTransaction(long id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // PUT: api/Transactions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransaction(long id, Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.TransactionID)
            {
                return BadRequest();
            }

            db.Entry(transaction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Transactions
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult PostTransaction(Transaction transaction)
        {

            var idempotentKey = String.Join(" ", Request.Headers.GetValues("idem-key").Select(x => x.ToUpper()));
            
            var State = db.Transactions
                .Where(x => x.CustomerID == transaction.CustomerID && x.TransactionRef.ToString() == idempotentKey)
                .Select(x=>x.State)
                .FirstOrDefault();

            
          

            if (State == null || State.ToString() == "Active" )
            {
                transaction.State = "Pending";
                transaction.Created_Date = DateTime.Now;
                transaction.TransactionRef = new Guid(idempotentKey);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Transactions.Add(transaction);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (TransactionExists(transaction.TransactionID))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }
                return CreatedAtRoute("DefaultApi", new { id = transaction.TransactionID }, transaction);
            }

            else
            {
                //return CreatedAtRoute("DefaultApi", tranDetail.Select(x => x.TransactionID), tranDetail);
                return Content(HttpStatusCode.Accepted, "Content already there");
                //return CreatedAtRoute("DefaultApi",  tranDetail., tranDetail);

            }
        }

        // DELETE: api/Transactions/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult DeleteTransaction(long id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();

            return Ok(transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(long id)
        {
            return db.Transactions.Count(e => e.TransactionID == id) > 0;
        }
    }
}