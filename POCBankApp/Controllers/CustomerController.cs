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
    public class CustomerController : ApiController
    {
        private EFConnectionString db = new EFConnectionString();

        // GET: api/Customer
        public IQueryable<Customer_Detail> GetCustomer_Detail()
        {
            return db.Customer_Detail;
        }

        // GET: api/Customer/5
        [ResponseType(typeof(Customer_Detail))]
        public IHttpActionResult GetCustomer_Detail(long id)
        {
            Customer_Detail customer_Detail = db.Customer_Detail.Find(id);
            if (customer_Detail == null)
            {
                return NotFound();
            }

            return Ok(customer_Detail);
        }

        // PUT: api/Customer/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer_Detail(long id, Customer_Detail customer_Detail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer_Detail.CustomerID)
            {
                return BadRequest();
            }

            db.Entry(customer_Detail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Customer_DetailExists(id))
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

        // POST: api/Customer
        [ResponseType(typeof(Customer_Detail))]
        public IHttpActionResult PostCustomer_Detail(Customer_Detail customer_Detail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customer_Detail.Add(customer_Detail);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customer_Detail.CustomerID }, customer_Detail);
        }

        // DELETE: api/Customer/5
        [ResponseType(typeof(Customer_Detail))]
        public IHttpActionResult DeleteCustomer_Detail(long id)
        {
            Customer_Detail customer_Detail = db.Customer_Detail.Find(id);
            if (customer_Detail == null)
            {
                return NotFound();
            }

            db.Customer_Detail.Remove(customer_Detail);
            db.SaveChanges();

            return Ok(customer_Detail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Customer_DetailExists(long id)
        {
            return db.Customer_Detail.Count(e => e.CustomerID == id) > 0;
        }
    }
}