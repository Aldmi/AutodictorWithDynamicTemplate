using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Xml.Linq;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entitys;
using Library.Xml;

namespace DAL.XmlRaw.Repository
{
    public class XmlRawPathWaysRepository : IPathwaysRepository
    {
        private readonly XElement _xElement;
        private IEnumerable<Pathways> Pathways { get; set; }



        #region ctor

        public XmlRawPathWaysRepository(XElement xElement)
        {
            _xElement = xElement;
        }

        public XmlRawPathWaysRepository(string folderName, string fileName)
        {
            var xmlFile = XmlWorker.LoadXmlFile(folderName, fileName);
            if (xmlFile == null)
                throw new FileNotFoundException("Файл не найден Config/DynamicSound.xml");
            _xElement = xmlFile;
        }

        #endregion






        public Pathways GetById(int id)
        {
            throw new NotImplementedException();
        }



        public IEnumerable<Pathways> List()
        {
            return Pathways ?? (Pathways = ParseXmlFile());
        }



        private IEnumerable<Pathways> ParseXmlFile()
        {
            var pathWays = new List<Pathways>();
            try
            {
                foreach (var directXml in _xElement.Elements("Path"))
                {
                    var path = new Pathways
                    {
                        Id = int.Parse((string)directXml.Attribute("Id")),
                        Name = (string)directXml.Attribute("Name"),
                        НаНомерПуть = (string)directXml.Attribute("НаНомерПуть"),
                        НаНомерОмПути = (string)directXml.Attribute("НаНомерОмПути"),
                        СНомерОгоПути = (string)directXml.Attribute("СНомерОгоПути"),
                        Addition = (string)directXml.Attribute("Addition"),
                        Addition2 = (string)directXml.Attribute("Addition2")
                    };

                    pathWays.Add(path);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return pathWays;
        }



        public IEnumerable<Pathways> List(Expression<Func<Pathways, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Add(Pathways entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Pathways entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(Pathways entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<Pathways, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Pathways> entitys)
        {
            throw new NotImplementedException();
        }
    }
}