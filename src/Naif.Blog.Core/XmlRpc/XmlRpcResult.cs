//Inspired by the work of Michael McKenna
//https://michael-mckenna.com/implementing-xml-rpc-services-in-asp-net-mvc/

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Naif.Blog.Models;

namespace Naif.Blog.XmlRpc
{
    public class XmlRpcResult : ContentResult
    {
        public XmlRpcResult(Post post)
        {
            var info = new
            {
                description = post.Content,
                title = post.Title,
                dateCreated = post.PubDate,
                wp_slug = post.Slug,
                categories = post.Categories.ToArray(),
                mt_keywords = post.Keywords,
                postid = post.PostId,
                mt_excerpt = post.Excerpt,
                custom_fields = post.CustomFields
            };            

            Initialize(info);
        }

        public XmlRpcResult(IEnumerable<Post> posts)
        {
            List<object> list = new List<object>();

            foreach (var post in posts)
            {
                var info = new
                {
                    description = post.Content,
                    title = post.Title,
                    dateCreated = post.PubDate,
                    wp_slug = post.Slug,
                    postid = post.PostId
                };

                list.Add(info);
            }

            Initialize(list.ToArray());
        }
        
        public XmlRpcResult(object data)
        {
            Initialize(data);
        }

        private void Initialize(object data)
        {
            //Set content type to xml
            ContentType = "text/xml";

            //Serialise data into base.Content
            Content = SerialiseXmlRpcResponse(data).ToString();
        }

        private XDocument SerialiseXmlRpcResponse(object data)
        {
            var exception = data as Exception;

            if (exception != null)
            {
                return new XDocument(
                        new XElement("methodResponse",
                            new XElement("fault",
                                new XElement("value",
                                    new XElement("string", exception.Message)
                                )
                            )
                        )
                    );
            }
            else
            {
                return new XDocument(
                        new XElement("methodResponse",
                            new XElement("params",
                                new XElement("param", XmlRpcData.SerialiseValue(data))
                            )
                        )
                    );
            }
        }
    }
}


