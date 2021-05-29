using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Naif.Blog.Models;
using Naif.Blog.Models.Entities;
using Naif.Blog.Models.Extensions;
using Naif.Blog.XmlRpc;
using NUnit.Framework;

namespace Naif.Blog.Core.Tests
{
    public class XmlRpcDataTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Ignore("Just used for local debugging")]
        public void XmlRpcData_Serializes_Posts()
        {
            var post = new Post
            {
                BlogId = "testblog",
                Categories = new List<Category>
                {
                    new() { Name = "General"},
                    new() { Name = "Home"}
                },
                Content = "testcontent",
                Excerpt = "excerpt",
                Template = "template",
                SubTitle = "subtitle",
                Title = "title"
            };
            
            var result = new XmlRpcResult(post.ToXmlRpcPost());
            
            Assert.AreEqual("content", result.Content);
            
            Assert.Pass();
        }

        [Test]
        public void XmlRpcData_Deserializes_Posts()
        {
            var post = new Post
            {
                Author = "Author",
                Categories = new List<Category>
                {
                    new() { Name = "General"},
                    new() { Name = "Home"}
                },
                Content = "testcontent",
                Excerpt = "excerpt",
                SubTitle = "subtitle",
                Template = "template",
                Title = "title"
            };
            
            var result = new XmlRpcResult(post.ToXmlRpcPost());
            
            TextReader tr = new StringReader(result.Content);
            XDocument doc = XDocument.Load(tr);
            XElement value = doc.Element("methodResponse").Element("params").Element("param").Element("value");

            var xmlRpcPost = XmlRpcData.DeserializeValue(value, typeof(XmlRpcPost)) as XmlRpcPost;
            var actualPost = xmlRpcPost.ToPost(String.Empty);

            Assert.AreEqual(post.Categories.Count, actualPost.Categories.Count);
            Assert.AreEqual(post.Categories[0].Name, actualPost.Categories[0].Name);
            Assert.AreEqual(post.Categories[1].Name, actualPost.Categories[1].Name);
            Assert.AreEqual(post.Content, actualPost.Content);
            Assert.AreEqual(post.Excerpt, actualPost.Excerpt);
            Assert.AreEqual(post.SubTitle, actualPost.SubTitle);
            Assert.AreEqual(post.Template, actualPost.Template);
            Assert.AreEqual(post.Title, actualPost.Title);
        }
    }
}