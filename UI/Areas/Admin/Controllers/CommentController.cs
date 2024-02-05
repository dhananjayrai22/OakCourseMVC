using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class CommentController : BaseController
    {
        PostBLL bll = new PostBLL();
        // GET: Admin/Comment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnapprovedComments()
        {
            List<CommentDTO> commentlist = new List<CommentDTO>();
            commentlist = bll.GetComments();
            return View(commentlist);
        }

        public ActionResult ApprovedComments(int ID)
        {
            bll.ApprovedComment(ID);
            return RedirectToAction("UnapprovedComments", "Comment");
        }

        public ActionResult ApprovedComments2(int ID)
        {
            bll.ApprovedComment(ID);
            return RedirectToAction("AllComments", "Comment");
        }

        public ActionResult AllComments()
        {
            List<CommentDTO> commentlist = bll.GetAllComments();
            return View(commentlist);
        }

        public JsonResult DeleteComment(int ID)
        {
            bll.DeleteComment(ID);
            return Json("");
        }
    }
}