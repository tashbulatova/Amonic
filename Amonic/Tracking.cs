//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Amonic
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tracking
    {
        public int TrackID { get; set; }
        public System.DateTime Login { get; set; }
        public Nullable<System.DateTime> Logout { get; set; }
        public string Reason { get; set; }
        public int UserID { get; set; }
    
        public virtual Users Users { get; set; }
        public string LoginDate
        {
            get
            {
                return (Login.ToShortDateString());
            }
            set { }
        }
        public string LoginTime {

            get
            {
                return (Login.ToShortTimeString());
            }
            set { }
        }
        
        public string LogoutTime
        {

            get
            {
                if (Logout != null)
                {
                    return (Logout.Value.ToShortTimeString());
                }
                else
                {
                    return "*";
                }
            }
            set { }
        }
        public string TimeSpent
        {
            get
            {
                if (Logout != null)
                {
                    return ((Logout.Value).Subtract(Login)).ToString();
                }
                else
                    return "**";
            }
            set { }
        }

        
    }
}