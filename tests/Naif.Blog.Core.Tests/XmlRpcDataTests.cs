using System.IO;
using System.Xml.Linq;
using Naif.Blog.Models;
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
                Categories = new [] {"General", "Home"},
                Content = "testcontent",
                Excerpt = "excerpt",
                Template = "template",
                SubTitle = "subtitle",
                Title = "title"
            };
            
            var result = new XmlRpcResult(post);
            
            Assert.AreEqual("content", result.Content);
            
            Assert.Pass();
        }

        [Test]
        public void XmlRpcData_Deserializes_Posts()
        {
            var post = new Post
            {
                Author = "Author",
                Categories = new [] {"General", "Home"},
                Content = "testcontent",
                Excerpt = "excerpt",
                SubTitle = "subtitle",
                Template = "template",
                Title = "title"
            };
            
            var result = new XmlRpcResult(post);
            
            TextReader tr = new StringReader(result.Content);
            XDocument doc = XDocument.Load(tr);
            XElement value = doc.Element("methodResponse").Element("params").Element("param").Element("value");

            var actualPost = XmlRpcData.DeserializeValue(value, typeof(Post)) as Post;

            Assert.AreEqual(post.Categories, actualPost.Categories);
            Assert.AreEqual(post.Content, actualPost.Content);
            Assert.AreEqual(post.Excerpt, actualPost.Excerpt);
            Assert.AreEqual(post.SubTitle, actualPost.SubTitle);
            Assert.AreEqual(post.Template, actualPost.Template);
            Assert.AreEqual(post.Title, actualPost.Title);
        }
    }
}