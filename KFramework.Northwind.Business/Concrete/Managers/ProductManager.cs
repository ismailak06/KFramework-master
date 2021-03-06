﻿using KFramework.Northwind.Business.Abstract;
using KFramework.Northwind.Business.ValidationRules.FluentValidation;
using KFramework.Northwind.DataAccess.Abstract;
using KFramework.Northwind.Entities.Concrete;
using System.Collections.Generic;
using System.Transactions;
using KFramework.Core.Aspects.PostSharp.ValidationAspects;
using KFramework.Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using KFramework.Core.Aspects.PostSharp.LogAspects;
using KFramework.Core.Aspects.PostSharp.PerformanceAspects;
using PostSharp.Aspects.Dependencies;
using KFramework.Core.Aspects.PostSharp.AuthorizationAspects;
using System.Linq;
using AutoMapper;
using KFramework.Northwind.Entities.DTOs;

namespace KFramework.Northwind.Business.Concrete.Managers
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        private IMapper _mapper;

        public ProductManager(IProductDal productDal, IMapper mapper)
        {
            _productDal = productDal;
            _mapper = mapper;
        }

        //[LogAspect(typeof(DatabaseLogger))]
        //[LogAspect(typeof(FileLogger))]
        //[ExpectionLogAspect(typeof(DatabaseLogger))]
        [FluentValidationAspect(typeof(ProductValidator))]
        public Product Add(Product product)
        {
            return _productDal.Add(product);
        }
       
        [LogAspect(typeof(DatabaseLogger))]
        [LogAspect(typeof(FileLogger))]
        [PerformanceCounterAspect(2)]
        //[SecuredOperation(Roles="Admin,Student,Editor")]
        public List<ProductDto> GetAll()
        {
            var result = _mapper.Map<List<ProductDto>>(_productDal.GetList());
            return result;
        }
   
        public Product GetById(int id)
        {
            return _productDal.Get(p => p.ProductId == id);
        }

        
        public void TransactionalOperation(Product product1, Product product2)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _productDal.Add(product1);
                _productDal.Add(product2);
            }
        }

        public Product Update(Product product)
        {
            return _productDal.Update(product);
        }
    }
}
