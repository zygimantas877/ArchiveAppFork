using ArchiveApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveApp.Collections
{
    internal class Images : IEnumerable<Image>
    {
        private List<Image> imageList = new List<Image>();

        public Images() { }

        public Images(List<Image> images)
        {
            imageList = images;
        }

        public Image this[int index]
        {
            get { return imageList[index]; }
            set { imageList[index] = value; }
        }

        public IEnumerator<Image> GetEnumerator()
        {
            return imageList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count()
        {
            return imageList.Count;
        }

        public void Add(Image image)
        {
            imageList.Add(image);
        }
    }
}
