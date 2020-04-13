using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Schedule.Contracts.Data;

namespace Schedule.Base
{
    public class BaseCrudController<DbModel, ViewModel, Repository> : Controller 
        where Repository: IRepository<DbModel>
        where DbModel: class
        where ViewModel: class
    {
        protected readonly Repository _repository;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly IStringLocalizer _localizer;

        protected static class DefaultLocalizerKeys
        {
            public static string CreateException = "CreateException";
            public static string CreateNotCreated = "CreateNotCreated";
            public static string UpdateException = "UpdateException";
            public static string UpdateNotUpdated = "UpdateNotUpdated";
            public static string DeleteException = "DeleteException";
            public static string DeleteNotDeleted = "DeleteNotDeleted";
        }

        public BaseCrudController(Repository repository, IMapper mapper, ILogger logger,
            IStringLocalizer localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _localizer = localizer;
        }
        
        // GET: BaseCrud
        public virtual async Task<ActionResult> Index()
        {
            var model = await getModelForListView();
            return View(model);
        }

        protected virtual async Task<IEnumerable<ViewModel>> getModelForListView()
        {
            var all = await _repository.GetAll();
            var model = _mapper.Map<IEnumerable<ViewModel>>(all);
            return model;
        }

        // GET: BaseCrud/Details/5
        public virtual async Task<ActionResult> Details(int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ViewModel>(entity);
            return View(model);
        }

        // GET: BaseCrud/Create
        public virtual async Task<ActionResult> Create()
        {
            return View();
        }

        protected virtual async Task<BeforeCreateResult<DbModel>> beforeCreate(DbModel entity)
        {
            return new BeforeCreateResult<DbModel>
            {
                Entity = entity,
                ShouldCreate = true,
            };
        }

        protected virtual async Task afterCreate(bool isCreated)
        {
            return;
        } 

        // POST: BaseCrud/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var dbModel = _mapper.Map<DbModel>(vm);
                var beforeCreateResult = await beforeCreate(dbModel);

                if (!beforeCreateResult.ShouldCreate)
                {
                    ModelState.AddModelError("", beforeCreateResult.ShouldNotCreateReason);
                    return View(vm);
                }
                else
                {
                    dbModel = beforeCreateResult.Entity;
                }

                var isCreated = await _repository.Create(dbModel);

                await afterCreate(isCreated);

                if (!isCreated)
                {
                    var errorMsg = _localizer.GetString(DefaultLocalizerKeys.CreateNotCreated);
                    ModelState.AddModelError("", errorMsg);
                    return View(vm);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception exc)
            {
                var errorMsg = _localizer.GetString(DefaultLocalizerKeys.CreateException);
                _logger.LogError($"Error in Create: {exc}");
                ModelState.AddModelError("", errorMsg);
                return View(vm);
            }
        }

        protected virtual async Task<BeforeUpdateResult<DbModel>> beforeUpdate(DbModel entity)
        {
            return new BeforeUpdateResult<DbModel>
            {
                Entity = entity,
                ShouldUpdate = true,
            };
        }

        protected virtual async Task afterUpdated(bool isUpdated)
        {
            return;
        }

        // GET: BaseCrud/Edit/5
        public virtual async Task<ActionResult> Update(int id)
        {
            return View();
        }

        // POST: BaseCrud/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Update(int id, ViewModel vm)
        {
            try
            {
                if (! await _repository.ExistsWithId(id))
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var dbModel = _mapper.Map<DbModel>(vm);
                var beforeUpdateResult = await beforeUpdate(dbModel);

                if (!beforeUpdateResult.ShouldUpdate)
                {
                    ModelState.AddModelError("", beforeUpdateResult.ShouldNotUpdateReason);
                    return View(vm);
                }
                else
                {
                    dbModel = beforeUpdateResult.Entity;
                }

                var isUpdated = await _repository.Update(dbModel);

                await afterUpdated(isUpdated);
                if (!isUpdated)
                {
                    var errorMsg = _localizer.GetString(DefaultLocalizerKeys.UpdateNotUpdated);
                    ModelState.AddModelError("", errorMsg);
                    return View(vm);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception exc)
            {
                _logger.LogError($"Update {id}: {exc}");
                var errorMsg = _localizer.GetString(DefaultLocalizerKeys.UpdateException);
                ModelState.AddModelError("", errorMsg);
                return View(vm);
            }
        }

        protected virtual async Task<BeforeDeleteResult<DbModel>> beforeDelete(DbModel entity)
        {
            return new BeforeDeleteResult<DbModel>
            {
                ShouldDelete = true,
                Entity = entity,
                ShouldNotDeleteReason = "",
            };
        }

        protected virtual async Task afterDelete(bool isDeleted)
        {
            return;
        }

        // GET: BaseCrud/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return View();
        }

        // POST: BaseCrud/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var entity = await _repository.GetById(id);
                if (entity == null)
                {
                    return NotFound();
                }

                var beforeDeleteResult = await beforeDelete(entity);

                if (!beforeDeleteResult.ShouldDelete)
                {
                    return Forbid(beforeDeleteResult.ShouldNotDeleteReason);
                }
                else
                {
                    entity = beforeDeleteResult.Entity;
                }

                var isDeleted = await _repository.Delete(entity);

                await afterDelete(isDeleted);

                if (!isDeleted)
                {
                    var errorMsg = _localizer.GetString(DefaultLocalizerKeys.DeleteNotDeleted);
                    return StatusCode(StatusCodes.Status500InternalServerError, errorMsg);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception exc)
            {
                _logger.LogError($"Delete {id}: {exc}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        protected class BeforeCreateResult<T>
        {
            public T Entity { get; set; }
            public bool ShouldCreate { get; set; }
            public string ShouldNotCreateReason { get; set; }
        }

        protected class BeforeUpdateResult<T>
        {
            public T Entity { get; set; }
            public bool ShouldUpdate { get; set; }
            public string ShouldNotUpdateReason { get; set; }
        }

        protected class BeforeDeleteResult<T>
        {
            public T Entity { get; set; }
            public bool ShouldDelete { get; set; }
            public string ShouldNotDeleteReason { get; set; }
        }
    }
}