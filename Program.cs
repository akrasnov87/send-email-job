using Email.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Mail;

namespace Email
{
    class Program
    { 
        static void Main(string[] args)
        {
            Program p = new Program();
            Console.WriteLine("start processing: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

            MailAddress from = new MailAddress("mysmtp1987@gmail.com", "Александр Краснов");

            // передача данных главам
            p.SendToMain(args[0], from);

            // передача данных ответственным за районы
            p.SendToDivision(args[0], from);
            
            // передача ответственным за округ
            p.SendToSubDivision(args[0], from);

            // передача ответственным за УИК
            p.SendToUik(args[0], from);

            Console.WriteLine("send finished" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
        }

        /// <summary>
        /// передача данных ответственным за округ
        /// </summary>
        /// <param name="dbName">имя бд для подключения</param>
        /// <param name="from"></param>
        void SendToUik(string dbName, MailAddress from)
        {
            UserInDivisionExtension[] users = getUikUsers(dbName);

            foreach (UserInDivisionExtension user in users)
            {
                if (!string.IsNullOrEmpty(user.c_email))
                {
                    List<PentahoUrlBuilder> reports = new List<PentahoUrlBuilder>();
                    reports.Add(new PentahoUrlBuilder("work_uik", "Ежедневный отчет по УИК в разрезе агитаторов - выход агитаторов на участок работы", "f_main_division=-1&f_division=-1&n_gos_subdivision=-1&n_uik=" + user.f_uik));

                    reports.Add(new PentahoUrlBuilder("count_day_uik", "Ежедневный отчет по УИК - результаты ОДД Агитаторов", "n_uik=" + user.f_uik));
                    reports.Add(new PentahoUrlBuilder("count_period_uik", "Сводный отчет по УИК - результаты ОДД Агитаторов", "n_uik=" + user.f_uik));

                    string[] emails = user.c_email.Trim().Replace(",", ";").Replace(" ", ";").Split(';');

                    Utilits.SendToMails(from, user.c_login, emails, reports, "УИК", user.f_uik.ToString());
                }
            }
        }

        /// <summary>
        /// передача данных ответственным за округ
        /// </summary>
        /// <param name="dbName">имя бд для подключения</param>
        /// <param name="from"></param>
        void SendToSubDivision(string dbName, MailAddress from)
        {
            UserInDivisionExtension[] users = getSubDivisionUsers(dbName);

            foreach (UserInDivisionExtension user in users)
            {
                if (!string.IsNullOrEmpty(user.c_email))
                {
                    List<PentahoUrlBuilder> reports = new List<PentahoUrlBuilder>();
                    reports.Add(new PentahoUrlBuilder("work_uik", "Ежедневный отчет по УИК в разрезе агитаторов - выход агитаторов на участок работы", "f_main_division=-1&f_division=" + user.f_division + "&n_gos_subdivision=" + user.n_gos_subdivision + "&n_uik=-1"));

                    reports.Add(new PentahoUrlBuilder("count_day_sub", "Ежедневный окружной отчет по результатам ОДД Агитаторов", "f_division=" + user.f_division + "&n_gos_subdivision=" + user.n_gos_subdivision));
                    reports.Add(new PentahoUrlBuilder("count_period_sub", "Сводный окружной отчет по результатам ОДД Агитаторов", "f_division=" + user.f_division + "&n_gos_subdivision=" + user.n_gos_subdivision));

                    string[] emails = user.c_email.Trim().Replace(",", ";").Replace(" ", ";").Split(';');

                    Utilits.SendToMails(from, user.c_login, emails, reports, "окружной", user.f_division.ToString() + "-" + user.n_gos_subdivision.ToString());
                }
            }
        }

        /// <summary>
        /// передача данных ответственным за районы
        /// </summary>
        /// <param name="dbName">имя бд для подключения</param>
        /// <param name="from"></param>
        void SendToDivision(string dbName, MailAddress from)
        {
            UserInDivisionExtension[] users = getDivisionUsers(dbName);

            foreach (UserInDivisionExtension user in users)
            {
                if (!string.IsNullOrEmpty(user.c_email))
                {
                    List<PentahoUrlBuilder> reports = new List<PentahoUrlBuilder>();
                    reports.Add(new PentahoUrlBuilder("work_uik", "Ежедневный отчет по УИК в разрезе агитаторов - выход агитаторов на участок работы", "f_main_division=" + (user.f_division == 10 ? user.f_division : 0) + "&f_division=" + user.f_division + "&n_gos_subdivision=-1&n_uik=-1"));

                    if (user.f_division != 10)
                    {
                        reports.Add(new PentahoUrlBuilder("count_day_div", "Ежедневный районный отчет по результатам ОДД Агитаторов", "f_division=" + user.f_division));
                        reports.Add(new PentahoUrlBuilder("count_period_div", "Сводный районный отчет по результатам ОДД Агитаторов", "f_division=" + user.f_division));
                    }
                    else
                    {
                        // это новочебоксарск
                        reports.Add(new PentahoUrlBuilder("count_day_nov", "Ежедневный городской (НВЧ) отчет по результатам ОДД Агитаторов"));
                        reports.Add(new PentahoUrlBuilder("count_period_nov", "Сводный городской (НВЧ) отчет по результатам ОДД Агитаторов"));
                    }

                    string[] emails = user.c_email.Trim().Replace(",", ";").Replace(" ", ";").Split(';');

                    Utilits.SendToMails(from, user.c_login, emails, reports, "районный", user.f_division.ToString());
                }
            }
        }

        /// <summary>
        /// передача данных главам
        /// </summary>
        /// <param name="dbName">имя бд для подключения</param>
        /// <param name="from"></param>
        void SendToMain(string dbName, MailAddress from)
        {
            // передача данных главам
            List<PentahoUrlBuilder> reports = new List<PentahoUrlBuilder>();
            reports.Add(new PentahoUrlBuilder("work_uik", "Ежедневный отчет по УИК в разрезе агитаторов - выход агитаторов на участок работы", "f_main_division=-1&f_division=-1&n_gos_subdivision=-1&n_uik=-1"));
            reports.Add(new PentahoUrlBuilder("count_day_cheb", "Ежедневный городской (ЧЕБ) отчет по результатам ОДД Агитаторов"));
            reports.Add(new PentahoUrlBuilder("count_period_cheb", "Сводный городской (ЧЕБ) отчет по результатам ОДД Агитаторов"));
            reports.Add(new PentahoUrlBuilder("count_day_nov", "Ежедневный городской (НВЧ) отчет по результатам ОДД Агитаторов"));
            reports.Add(new PentahoUrlBuilder("count_period_nov", "Сводный городской (НВЧ) отчет по результатам ОДД Агитаторов"));
            User[] users = getMainUsers(dbName);

            foreach (User user in users)
            {
                if (!string.IsNullOrEmpty(user.c_email))
                {
                    string[] emails = user.c_email.Trim().Replace(",", ";").Replace(" ", ";").Split(';');

                    Utilits.SendToMails(from, user.c_login, emails, reports, "городской", "0");
                }
            }
        }

        /// <summary>
        /// Получение списка главных пользователей (супер диспетчер)
        /// </summary>
        /// <param name="dbName">имя бд для подключения</param>
        /// <returns></returns>
        User[] getMainUsers(string dbName)
        {
            using(ApplicationContext db = new ApplicationContext(dbName))
            {
                var users = from uir in db.UserInRoles
                            where uir.f_role == 6 && !uir.sn_delete
                            select uir.f_user;

                var query = from uid in db.UserInDivisions
                            join u in db.Users on uid.f_user equals u.id
                            where !u.b_disabled && !uid.sn_delete && uid.f_division == null
                            && uid.f_subdivision == null
                            && uid.f_uik == null
                            && uid.n_gos_subdivision == null
                            && users.Contains(uid.f_user)
                            select u;

                return query.ToArray();
            }
        }

        /// <summary>
        /// Получение списка пользователей раойнов и новочебоксарска
        /// </summary>
        /// <param name="dbName">имя бд для подключения</param>
        /// <returns></returns>
        UserInDivisionExtension[] getDivisionUsers(string dbName)
        {
            using (ApplicationContext db = new ApplicationContext(dbName))
            {
                var users = from uir in db.UserInRoles
                            where uir.f_role == 6 && !uir.sn_delete
                            select uir.f_user;

                var query = from uid in db.UserInDivisions
                            join u in db.Users on uid.f_user equals u.id
                            where !u.b_disabled && !uid.sn_delete && uid.f_division != null
                            && uid.f_subdivision == null
                            && uid.f_uik == null
                            && uid.n_gos_subdivision == null
                            && users.Contains(uid.f_user)
                            orderby uid.f_division
                            select new UserInDivisionExtension()
                            {
                                c_email = u.c_email,
                                id = u.id,
                                c_login = u.c_login,
                                f_division = uid.f_division,
                                f_subdivision = uid.f_subdivision,
                                f_uik = uid.f_uik,
                                n_gos_subdivision = uid.n_gos_subdivision
                            };

                return query.ToArray();
            }
        }

        /// <summary>
        /// Получение списка пользователей ответственных за округа
        /// </summary>
        /// <param name="dbName">имя бд для подключения</param>
        /// <returns></returns>
        UserInDivisionExtension[] getSubDivisionUsers(string dbName)
        {
            using (ApplicationContext db = new ApplicationContext(dbName))
            {
                var users = from uir in db.UserInRoles
                            where uir.f_role == 6 && !uir.sn_delete
                            select uir.f_user;

                var query = from uid in db.UserInDivisions
                            join u in db.Users on uid.f_user equals u.id
                            where !u.b_disabled && !uid.sn_delete && uid.f_division != null
                            && uid.f_subdivision == null
                            && uid.f_uik == null
                            && uid.n_gos_subdivision != null
                            && users.Contains(uid.f_user)
                            orderby uid.n_gos_subdivision
                            select new UserInDivisionExtension()
                            {
                                c_email = u.c_email,
                                id = u.id,
                                c_login = u.c_login,
                                f_division = uid.f_division,
                                f_subdivision = uid.f_subdivision,
                                f_uik = uid.f_uik,
                                n_gos_subdivision = uid.n_gos_subdivision
                            };

                return query.ToArray();
            }
        }

        /// <summary>
        /// Получение списка пользователей ответственных за УИК
        /// </summary>
        /// <param name="dbName">имя бд для подключения</param>
        /// <returns></returns>
        UserInDivisionExtension[] getUikUsers(string dbName)
        {
            using (ApplicationContext db = new ApplicationContext(dbName))
            {
                var users = from uir in db.UserInRoles
                            where uir.f_role == 6 && !uir.sn_delete
                            select uir.f_user;

                var query = from uid in db.UserInDivisions
                            join u in db.Users on uid.f_user equals u.id
                            where !u.b_disabled && !uid.sn_delete && uid.f_division == null
                            && uid.f_subdivision == null
                            && uid.f_uik != null
                            && uid.n_gos_subdivision == null
                            && users.Contains(uid.f_user)
                            orderby uid.f_uik
                            select new UserInDivisionExtension()
                            {
                                c_email = u.c_email,
                                id = u.id,
                                c_login = u.c_login,
                                f_division = uid.f_division,
                                f_subdivision = uid.f_subdivision,
                                f_uik = uid.f_uik,
                                n_gos_subdivision = uid.n_gos_subdivision
                            };

                return query.ToArray();
            }
        }

        class UserInDivisionExtension
        {
            public int id { get; set; }
            public string c_login { get; set; }
            public string c_email { get; set; }
            public int? f_division { get; set; }
            public int? f_subdivision { get; set; }
            public int? f_uik { get; set; }
            public int? n_gos_subdivision { get; set; }
        }
    }
}
