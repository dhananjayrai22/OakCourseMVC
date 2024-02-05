﻿using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PostBLL
    {
        PostDAO dao = new PostDAO();

        public bool AddComment(GeneralDTO model)
        {
            Comment comment = new Comment();
            comment.PostID = model.PostID;
            comment.NameSurname = model.Name;
            comment.Email = model.Email;
            comment.CommentContent = model.Message;
            comment.AddDate= DateTime.Now;
            dao.AddComment(comment);
            return true;
        }

        public bool AddPost(PostDTO model)
        {
            Post post = new Post();
            post.Title = model.Title;
            post.PostContent = model.PostContent;
            post.ShortContent = model.ShortContent;
            post.Slider = model.Slider;
            post.Area1 = model.Area1;
            post.Area2 = model.Area2;
            post.Area3 = model.Area3;
            post.Notification = model.Notification;
            post.CategoryID = model.CategoryID;
            post.SeoLink = SeoLink.GenerateUrl(model.Title);
            post.LanguageName = model.Language;
            post.AddDate = DateTime.Now;
            post.LastUpdateDate = DateTime.Now;
            post.LastUpdateUserID = UserStatic.UserID;
            post.AddUserID = UserStatic.UserID;
            int ID = dao.AddPost(post);
            LogDAO.AddLog(General.ProcessType.PostAdd, General.TableName.post, ID);
            SavePostImage(model.PostImages, ID);
            AddTag(model.TagText, ID);
            return true;

        }

        public void ApprovedComment(int ID)
        {
            dao.ApprovedComment(ID);
            LogDAO.AddLog(General.ProcessType.CommentApprove, General.TableName.Comment, ID);
        }

        public void DeleteComment(int ID)
        {
            dao.DeleteComment(ID);
            LogDAO.AddLog(General.ProcessType.CommentDelete, General.TableName.Comment, ID);
        }

        public List<PostImageDTO> DeletePost(int ID)
        {
            List<PostImageDTO> imagelist = dao.DeletePost(ID);
            LogDAO.AddLog(General.ProcessType.PostDelete, General.TableName.post, ID);
            return imagelist;
        }

        public string DeletePostImage(int ID)
        {
            string Imagepath = dao.DeletePostImage(ID);
            LogDAO.AddLog(General.ProcessType.ImageDelete, General.TableName.Image, ID);
            return Imagepath;
        }

        public List<CommentDTO> GetAllComments()
        {
            return dao.GetAllComments();
        }

        public CountDTO GetAllCounts()
        {
            return dao.GetAllCounts();
        }

        public List<CommentDTO> GetComments()
        {
            return dao.GetComments();
        }

        public CountDTO GetCounts()
        {
            CountDTO dto = new CountDTO();
            dto.MessageCount = dao.GetMessageCount();
            //dto.CommentCount = dao.GetCommentCount();
            return dto;
        }

        public List<PostDTO> GetPosts()
        {
            return dao.GetPosts();
        }

        public PostDTO GetPostWithID(int ID)
        {
            PostDTO dto = new PostDTO();
            dto = dao.GetPostWithID(ID);
            dto.PostImages = dao.GetPostImagesWithID(ID);
            List<PostTag> taglist = dao.GetPostTagwithPostID(ID);
            string Tagvalue = "";
            foreach (var item in taglist)
            {
                Tagvalue += item.TagContent;
                Tagvalue += ",";
            }
            dto.TagText = Tagvalue;
            return dto;
        }

        public bool UpdatePost(PostDTO model)
        {
            model.SeoLink = SeoLink.GenerateUrl(model.Title);
            dao.UpdatePost(model);
            LogDAO.AddLog(General.ProcessType.PostUpdate, General.TableName.post, model.ID);
            if (model.PostImages != null)
                SavePostImage(model.PostImages, model.ID);
            dao.DeleteTags(model.ID);
            AddTag(model.TagText, model.ID);
            return true;
        }

        private void AddTag(string tagText, int PostID)
        {
            if (tagText != null)
            {
                string[] tags;
                tags = tagText.Split(',');
                List<PostTag> taglist = new List<PostTag>();
                foreach (var item in tags)
                {
                    PostTag tag = new PostTag();
                    tag.PostID = PostID;
                    tag.TagContent = item;
                    tag.AddDate = DateTime.Now;
                    tag.LastUpdateUserID = UserStatic.UserID;
                    tag.LastUpdateDate = DateTime.Now;
                    taglist.Add(tag);
                }
                foreach (var item in taglist)
                {
                    int tagID = dao.AddTag(item);
                    LogDAO.AddLog(General.ProcessType.TagAdd, General.TableName.Tag, tagID);
                }
            }
        }

            void SavePostImage(List<PostImageDTO> list, int PostID)
            {
                List<PostImage> imagelist = new List<PostImage>();
                foreach (var item in list)
                {
                    PostImage image = new PostImage();
                    image.ID = PostID;
                    image.ImagePath = item.ImagePath;
                    image.AddDate = DateTime.Now;
                    image.LastUpdateDate = DateTime.Now;
                    image.LastUpdateUserID = UserStatic.UserID;
                    imagelist.Add(image);
                }
                foreach (var item in imagelist)
                {
                    int ImageID = dao.AddImage(item);
                    LogDAO.AddLog(General.ProcessType.ImageAdd, General.TableName.Image, ImageID);
                }
            }
        }
    }

