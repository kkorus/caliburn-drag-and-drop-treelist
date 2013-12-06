using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaliburnApp.Domain.Contracts;
using CaliburnApp.Domain.Entities;

namespace CaliburnApp.Domain.Services
{
    public class BusinessValueObjectsService : IBusinessValueObjectsService
    {
        private IRepository<BusinessValueObject> _repository;

        public BusinessValueObjectsService(IRepository<BusinessValueObject> repository)
        {
            _repository = repository;
        }

        public IEnumerable<BusinessValueObject> Items()
        {
            return _repository.Items();
        }

        public void ChangeParent(int id, int parentId, bool isCopying = false)
        {
            var businessObject = _repository.GetById(id);
            var parent = _repository.GetById(parentId);
            businessObject.ChangeParent(parent);

            //if(delete)

            _repository.SaveChanges();
       }
    }
}
