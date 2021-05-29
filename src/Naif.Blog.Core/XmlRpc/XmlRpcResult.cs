//Inspired by the work of Michael McKenna
//https://michael-mckenna.com/implementing-xml-rpc-services-in-asp-net-mvc/

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Naif.Blog.Models.Entities;
// ReSharper disable UsePatternMatching

namespace Naif.Blog.XmlRpc
{
    public class XmlRpcResult : ContentResult
    {
        internal XmlRpcResult(XmlRpcPost xmlRpcPost)
        {
            var info = new
            {
                description = xmlRpcPost.Content,
                title = xmlRpcPost.Title,
                dateCreated = xmlRpcPost.PubDate,
                wp_slug = xmlRpcPost.Slug,
                categories = xmlRpcPost.Categories,
                mt_keywords = xmlRpcPost.Keywords,
                postid = xmlRpcPost.PostId,
                mt_excerpt = xmlRpcPost.Excerpt,
                custom_fields = xmlRpcPost.CustomFields
            };            

            Initialize(info);
        }

        internal XmlRpcResult(IEnumerable<XmlRpcPost> xmlRpcPosts)
        {
            List<object> list = new List<object>();

            foreach (var post in xmlRpcPosts)
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
        
        internal XmlRpcResult(object data)
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


