using System;
using System.Collections.Generic;
using System.Text;

namespace BOL
{
    public class Menu: IComparable
    {

        private string _caption = string.Empty;
        private string _detailCaption = string.Empty;
        private int _id = -1;
     
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        

        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
            }
        }

        public string DetailCaption
        {
            get
            {
                return _detailCaption;
            }
            set
            {
                _detailCaption = value;
            }
        }

        //public string GroupName
        //{
        //    //get
        //    {
        //        return _groupName;
        //    }
        //    set
        //    {
        //        _groupName = value;
        //    }
        //}

        public int CompareTo(object obj)
        {
            return ID.CompareTo(obj);
        }

        public int CompareTo(string obj)
        {
            return ID.CompareTo(obj);
        }

    }

    public class MenuGroup : List<Menu>
    {
        private string _groupName = string.Empty;
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
            }
        }

        public MenuGroup()
        {

        }

        public MenuGroup(string groupName)
        {
            _groupName = groupName;
        }
    }
}
