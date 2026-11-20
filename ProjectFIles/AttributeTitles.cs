using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormitoryXML.Models
{
   public static class AttributeTitles
    {
        public static string Get(string name) => name switch
        {
            "id" => "ID",
            "fullName" => "П.І.П.",
            "faculty" => "Факультет",
            "department" => "Кафедра",
            "group" => "Група",
            "course" => "Курс",
            "email" => "Email",
            "phone" => "Телефон",
            "scholarship" => "Стипендія",
            "birthYear" => "Рік народження",
            "citizenship" => "Громадянство",
            "note" => "Примітка",
            _ => name
        };
    }
}
