using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Schedule.Common.Services;
using Schedule.Common.Contracts.Data;
using Schedule.Data;
using Schedule.Common.Contracts;
using static Schedule.Views.Utils;

namespace Schedule.Common.Controllers
{
    public class BaseCrudController<TEntity, TViewModel> : Controller 
        where TEntity: Entity, new() 
        where TViewModel: class
    {
        protected readonly ICRUDService<TEntity> _crudService;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly IStringLocalizer _localizer;

        protected static class DefaultLocalizerKeys
        {
            public static readonly string CreateException = "CreateException";
            public static readonly string CreateNotCreated = "CreateNotCreated";
            public static readonly string CreateSuccess = "CreateSuccess";

            public static readonly string UpdateException = "UpdateException";
            public static readonly string UpdateNotUpdated = "UpdateNotUpdated";
            public static readonly string UpdateSuccess = "UpdateSuccess";

            public static readonly string DeleteException = "DeleteException";
            public static readonly string DeleteNotDeleted = "DeleteNotDeleted";
            public static readonly string DeleteSuccess = "DeleteSuccess";
        }

        public BaseCrudController(ICRUDService<TEntity> crudService, IMapper mapper, ILogger logger,
            IStringLocalizer localizer)
        {
            _crudService = crudService;
            _mapper = mapper;
            _logger = logger;
            _localizer = localizer;
        }
        
        // GET: BaseCrud
        public virtual async Task<ActionResult> Index()
        {
            var model = await GetModelForListView();
            return View(model);
        }

        protected virtual async Task<IEnumerable<TViewModel>> GetModelForListView()
        {
            var all = await _crudService.GetAllAsync();
            var model = _mapper.Map<IEnumerable<TViewModel>>(all);
            return model;
        }

        // GET: BaseCrud/Details/5
        public virtual async Task<ActionResult> Details(int id)
        {
            var entity = _crudService.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<TViewModel>(entity);
            return View(model);
        }

        // GET: BaseCrud/Create
        public virtual async Task<ActionResult> Create()
        {
            return View();
        }

        protected virtual async Task<BeforeCreateResult<TEntity>> BeforeCreate(TEntity entity)
        {
            return new BeforeCreateResult<TEntity>
            {
                Entity = entity,
                ShouldCreate = true,
            };
        }

        protected virtual async Task AfterCreate(bool isCreated)
        {
            return;
        } 

        // POST: BaseCrud/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var dbModel = _mapper.Map<TEntity>(vm);
                var beforeCreateResult = await BeforeCreate(dbModel);

                if (!beforeCreateResult.ShouldCreate)
                {
                    TempData[FlashMessagesKeys.Error] = beforeCreateResult.ShouldNotCreateReason;
                    return View(vm);
                }
                else
                {
                    dbModel = beforeCreateResult.Entity;
                }

                var isCreated = await _crudService.CreateAsync(dbModel);

                await AfterCreate(isCreated);

                if (!isCreated)
                {
                    var errorMsg = _localizer.GetString(DefaultLocalizerKeys.CreateNotCreated);
                    TempData[FlashMessagesKeys.Error] = errorMsg;
                    return View(vm);
                }

                TempData[FlashMessagesKeys.Success] = DefaultLocalizerKeys.CreateSuccess;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception exc)
            {
                var errorMsg = _localizer.GetString(DefaultLocalizerKeys.CreateException);
                _logger.LogError($"Error in Create: {exc}");
                TempData[FlashMessagesKeys.Error] = errorMsg;
                return View(vm);
            }
        }

        protected virtual async Task<BeforeUpdateResult<TEntity>> BeforeUpdate(TEntity entity)
        {
            return new BeforeUpdateResult<TEntity>
            {
                Entity = entity,
                ShouldUpdate = true,
            };
        }

        protected virtual async Task AfterUpdated(bool isUpdated)
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
        public virtual async Task<ActionResult> Update(int id, TViewModel vm)
        {
            try
            {
                if (! await _crudService.ExistsWithIdAsync(id))
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var dbModel = _mapper.Map<TEntity>(vm);
                var beforeUpdateResult = await BeforeUpdate(dbModel);

                if (!beforeUpdateResult.ShouldUpdate)
                {
                    TempData[FlashMessagesKeys.Error] = beforeUpdateResult.ShouldNotUpdateReason;
                    return View(vm);
                }
                else
                {
                    dbModel = beforeUpdateResult.Entity;
                }

                var isUpdated = await _crudService.UpdateAsync(dbModel);

                await AfterUpdated(isUpdated);
                if (!isUpdated)
                {
                    var errorMsg = _localizer.GetString(DefaultLocalizerKeys.UpdateNotUpdated);
                    TempData[FlashMessagesKeys.Error] = errorMsg;
                    return View(vm);
                }

                TempData[FlashMessagesKeys.Success] = DefaultLocalizerKeys.UpdateSuccess;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception exc)
            {
                _logger.LogError($"Update {id}: {exc}");
                var errorMsg = _localizer.GetString(DefaultLocalizerKeys.UpdateException);
                TempData[FlashMessagesKeys.Error] = errorMsg;
                return View(vm);
            }
        }

        protected virtual async Task<BeforeDeleteResult<TEntity>> BeforeDelete(TEntity entity)
        {
            return new BeforeDeleteResult<TEntity>
            {
                ShouldDelete = true,
                Entity = entity,
                ShouldNotDeleteReason = "",
            };
        }

        protected virtual async Task AfterDelete(bool isDeleted)
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
                var entity = await _crudService.GetByIdAsync(id);
                if (entity == null)
                {
                    return NotFound();
                }

                var beforeDeleteResult = await BeforeDelete(entity);

                if (!beforeDeleteResult.ShouldDelete)
                {
                    return Forbid(beforeDeleteResult.ShouldNotDeleteReason);
                }
                else
                {
                    entity = beforeDeleteResult.Entity;
                }

                var isDeleted = await _crudService.DeleteAsync(entity);

                await AfterDelete(isDeleted);

                if (!isDeleted)
                {
                    var errorMsg = _localizer.GetString(DefaultLocalizerKeys.DeleteNotDeleted);
                    // TempData[FlashMessagesKeys.Error] = errorMsg;
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