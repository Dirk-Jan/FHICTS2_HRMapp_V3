﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMapp.Logic;
using HRMapp.Models.Exceptions;
using HRMapp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HRMapp.Controllers
{
    public class SkillsetController : Controller
    {
        private static CrossActionMessageHolder infoMessage = new CrossActionMessageHolder();
        private SkillsetLogic skillsetLogic = new SkillsetLogic();

        public IActionResult Index(int id)
        {
            var skillsets = skillsetLogic.GetAll().ToList();    // Where do I use a List and where an IEnumerable? Where do I convert?
            if (id == 0 && skillsets.Count > 0)                 // 0 Is the default id, no parameter passed
            {
                id = skillsets[0].Id;
            }
            var model = new SkillsetCollectionViewModel(id, skillsets) { InfoMessage = infoMessage.Message };    
            return View("Skillset", model);
        }

        public IActionResult SkillsetView(int id)
        {
            var skillset = skillsetLogic.GetById(id);
            return PartialView("_SkillsetView", skillset);
        }

        public IActionResult New()
        {
            return View("SkillsetEditor", new SkillsetEditorViewModel());
        }

        [HttpPost]
        public IActionResult New(SkillsetEditorViewModel model)
        {
            try
            {
                var addedSkillsetId = skillsetLogic.Add(model.ToSkillset());
                infoMessage.Message = $"'{model.Name}' is toegevoegd aan het systeem.";
                return RedirectToAction("Index", new {id = addedSkillsetId});
            }
            catch (ArgumentException argEx)
            {
                model.ErrorMessage = argEx.Message;
                model.EditorType = EditorType.New;
                return View("SkillsetEditor", model);
            }
            catch (DBException dbEx)
            {
                model.ErrorMessage = dbEx.Message;
                model.EditorType = EditorType.New;          // Dit hoeft niet, maar zou ik het er bij zetten zodat de code beter te begrijpen is? Wat werkt het beste voor mij?
                return View("SkillsetEditor", model);
            }
        }

        public IActionResult Edit(int id)
        {
            var skillset = skillsetLogic.GetById(5);
            return View("SkillsetEditor", new SkillsetEditorViewModel(skillset));
        }

        [HttpPost]
        public IActionResult Edit(SkillsetEditorViewModel model)
        {
            try
            {
                skillsetLogic.Update(model.ToSkillset());
                infoMessage.Message = $"'{model.Name}' is bewerkt.";
                return RedirectToAction("Index", new {id = model.Id});
            }
            catch (ArgumentException argEx)
            {
                model.ErrorMessage = argEx.Message;
                model.EditorType = EditorType.Edit;
                return View("SkillsetEditor", model);
            }
            catch (DBException dbbEx)
            {
                model.ErrorMessage = dbbEx.Message;
                model.EditorType = EditorType.Edit;
                return View("SkillsetEditor", model);
            }
        }
    }
}