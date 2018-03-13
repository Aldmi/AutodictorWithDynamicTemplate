using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private IEnumerable<Pathway> Pathways { get; set; }




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




        #region Methode

        public Pathway GetById(int id)
        {
            return List().FirstOrDefault(path => path.Id == id);
        }


        public IEnumerable<Pathway> List()
        {
            return Pathways ?? (Pathways = ParseXmlFile());
        }


        private IEnumerable<Pathway> ParseXmlFile()
        {
            var pathWays = new List<Pathway>();
            try
            {
                foreach (var directXml in _xElement.Elements("Path"))
                {
                    var path = new Pathway
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



        public IEnumerable<Pathway> List(Expression<Func<Pathway, bool>> predicate)
        {
            return List().Where(predicate.Compile());
        }

        public void Add(Pathway entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Pathway entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(Pathway entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<Pathway, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Pathway> entitys)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}