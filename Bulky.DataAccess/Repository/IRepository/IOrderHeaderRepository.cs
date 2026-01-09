using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        //paymentStatus is nullable because when we update the status the payment status can be the same
        void UpdateStatus(int id,string orderStatus,string? paymentStatus = null);
        void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId);

    }
}
