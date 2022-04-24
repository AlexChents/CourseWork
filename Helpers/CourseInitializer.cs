using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CourseChentsov.Models;
using System.Security.Cryptography;
using System.Text;

namespace CourseChentsov.Helpers
{
    public class CourseInitializer: DropCreateDatabaseAlways<CourseContext>
    {
        protected override void Seed(CourseContext context)
        {
            //области
            context.Regions.Add(new Region { RegionName = "Винницкая обл." });
            context.Regions.Add(new Region { RegionName = "Днепропетровская обл." });
            context.Regions.Add(new Region { RegionName = "Донецкая обл." });
            context.Regions.Add(new Region { RegionName = "Житомирская обл." });
            context.Regions.Add(new Region { RegionName = "Закарпатская обл." });
            context.Regions.Add(new Region { RegionName = "Запорожская обл." });
            context.Regions.Add(new Region { RegionName = "Ивано-Франковская обл." });
            context.Regions.Add(new Region { RegionName = "Киевская обл." });
            context.Regions.Add(new Region { RegionName = "Кировоградская обл." });
            context.Regions.Add(new Region { RegionName = "Луганская обл." });
            context.Regions.Add(new Region { RegionName = "Волынская обл." });
            context.Regions.Add(new Region { RegionName = "Львовская обл." });
            context.Regions.Add(new Region { RegionName = "Николаевская обл." });
            context.Regions.Add(new Region { RegionName = "Одесская обл." });
            context.Regions.Add(new Region { RegionName = "Полтавская обл." });
            context.Regions.Add(new Region { RegionName = "Ровенская обл." });
            context.Regions.Add(new Region { RegionName = "Сумская обл." });
            context.Regions.Add(new Region { RegionName = "Тернопольская обл." });
            context.Regions.Add(new Region { RegionName = "Харьковская обл." });
            context.Regions.Add(new Region { RegionName = "Херсонская обл." });
            context.Regions.Add(new Region { RegionName = "Хмельницкая обл." });
            context.Regions.Add(new Region { RegionName = "Черкасская обл." });
            context.Regions.Add(new Region { RegionName = "Черновицкая обл." });
            context.Regions.Add(new Region { RegionName = "Черниговская обл." });

            context.SaveChanges();


            for (int i = 1; i <= 3; i++)
            {
                context.BasicPriceDaysDeliveries.Add(new BasicPriceDaysDelivery { BasicPrice = i * 20 + 30, PriceForKg = i - i * 0.5 + 1.5, CountDays = i });
            }
            context.SaveChanges();

            //дистанция между областями
            Random random = new Random();
            int countRegions = context.Regions.Count();

            for (int i = 1; i <= countRegions; i++)
                for (int j = i; j <= countRegions; j++)
                    context.DeliveryDistances.Add(new DistanceDelivery { FirstRegionId = i, SecondRegionId = j, BasicPriceDaysDeliveryId = random.Next(1, 4) });
            
            //города

            context.Cities.Add(new City { Name = "Винница", RegionId = 1 });
            context.Cities.Add(new City { Name = "Днепр", RegionId = 2 });
            context.Cities.Add(new City { Name = "Краматорск", RegionId = 3 });
            context.Cities.Add(new City { Name = "Житомир", RegionId = 4 });
            context.Cities.Add(new City { Name = "Ужгород", RegionId = 5});
            context.Cities.Add(new City { Name = "Запорожье", RegionId = 6 });
            context.Cities.Add(new City { Name = "Ивано-Франковск", RegionId = 7 });
            context.Cities.Add(new City { Name = "Киев", RegionId = 8 });
            context.Cities.Add(new City { Name = "Кропивницкий", RegionId = 9 });
            context.Cities.Add(new City { Name = "Лисичанск", RegionId = 10});
            context.Cities.Add(new City { Name = "Луцк", RegionId = 11});
            context.Cities.Add(new City { Name = "Львов", RegionId = 12});
            context.Cities.Add(new City { Name = "Николаев", RegionId = 13});
            context.Cities.Add(new City { Name = "Одесса", RegionId = 14});
            context.Cities.Add(new City { Name = "Полтава", RegionId = 15});
            context.Cities.Add(new City { Name = "Ровно", RegionId = 16});
            context.Cities.Add(new City { Name = "Сумы", RegionId = 17});
            context.Cities.Add(new City { Name = "Тернополь", RegionId = 18});
            context.Cities.Add(new City { Name = "Харьков", RegionId = 19});
            context.Cities.Add(new City { Name = "Херсон", RegionId = 20});
            context.Cities.Add(new City { Name = "Хмельницкий", RegionId = 21});
            context.Cities.Add(new City { Name = "Черкассы", RegionId = 22});
            context.Cities.Add(new City { Name = "Черновцы", RegionId = 23});
            context.Cities.Add(new City { Name = "Чернигов", RegionId = 24});
            context.Cities.Add(new City { Name = "Кривой Рог", RegionId = 2 });
            context.Cities.Add(new City { Name = "Кременчуг", RegionId = 14 });
            context.Cities.Add(new City { Name = "Каменец-Подольский", RegionId = 21 });
            context.Cities.Add(new City { Name = "Павлоград", RegionId = 2 });
            context.Cities.Add(new City { Name = "Каменское", RegionId = 2 });
            context.Cities.Add(new City { Name = "Белая Церковь", RegionId = 8 });
            context.Cities.Add(new City { Name = "Белгород-Днестровский", RegionId = 14 });
            context.Cities.Add(new City { Name = "Борисполь", RegionId = 8 });
            context.Cities.Add(new City { Name = "Вишневое", RegionId = 8 });
            context.Cities.Add(new City { Name = "Васильков", RegionId = 8 });
            context.Cities.Add(new City { Name = "Энергодар", RegionId = 6 });
            context.Cities.Add(new City { Name = "Изюм", RegionId = 19 });
            context.Cities.Add(new City { Name = "Лубны", RegionId = 15 });
            context.Cities.Add(new City { Name = "Мелитополь", RegionId = 6 });
            context.Cities.Add(new City { Name = "Обухов", RegionId = 8 });
            context.Cities.Add(new City { Name = "Ахтырка", RegionId = 17 });
            context.Cities.Add(new City { Name = "Северодонецк", RegionId = 10 });
            context.Cities.Add(new City { Name = "Славянск", RegionId = 3 });
            context.Cities.Add(new City { Name = "Тростянец", RegionId = 1 });
            context.Cities.Add(new City { Name = "Черноморск", RegionId = 14 });
            context.Cities.Add(new City { Name = "Южноукраинск", RegionId = 14 });
            context.Cities.Add(new City { Name = "Гадяч", RegionId = 12 });
            context.Cities.Add(new City { Name = "Городок", RegionId = 12 });
            context.Cities.Add(new City { Name = "Геническ", RegionId = 20 });
            context.Cities.Add(new City { Name = "Гайсин", RegionId = 1 });
            context.Cities.Add(new City { Name = "Глухов", RegionId = 17 });
            context.Cities.Add(new City { Name = "Купянск", RegionId = 19 });
            context.Cities.Add(new City { Name = "Лозовая", RegionId = 19 });
            context.Cities.Add(new City { Name = "Волчанск", RegionId = 19 });
            context.Cities.Add(new City { Name = "Дрогобыч", RegionId = 12 });
            context.Cities.Add(new City { Name = "Здолбунов", RegionId = 16 });
            context.Cities.Add(new City { Name = "Золотоноша", RegionId = 21 });
            context.Cities.Add(new City { Name = "Нежин", RegionId = 24 });
            context.Cities.Add(new City { Name = "Красноград", RegionId = 19 });
            context.Cities.Add(new City { Name = "Коломыя", RegionId = 7 });
            context.Cities.Add(new City { Name = "Яремче", RegionId = 7 });
            context.Cities.Add(new City { Name = "Збараж", RegionId = 18 });
            context.Cities.Add(new City { Name = "Ковель", RegionId = 11 });

            context.SaveChanges();

            //графики работы отделений и их статус
            List<Schedule> schedules = new List<Schedule>
            {
                new Schedule { WeekdaysWork = "Пн-Пт", WeekdaysTimeWork = "08.00-20.00", SaturdayWork = "Сб", SaturdayTimeWork = "09.00-18.00", SundayWork = "Вс", SundayTimeWork = "10.00-16.00" },
                new Schedule { WeekdaysWork = "Пн-Пт", WeekdaysTimeWork = "08.00-20.00", SaturdayWork = "Сб", SaturdayTimeWork = "09.00-18.00", SundayWork = "Вс", SundayTimeWork = "Выходной"},
                new Schedule { WeekdaysWork = "Пн-Пт", WeekdaysTimeWork = "09.00-19.00", SaturdayWork = "Сб", SaturdayTimeWork = "10.00-16.00", SundayWork = "Вс", SundayTimeWork = "Выходной"},
                new Schedule { WeekdaysWork = "Пн-Пт", WeekdaysTimeWork = "09.00-18.00", SaturdayWork = "Сб", SaturdayTimeWork = "Выходной", SundayWork = "Вс", SundayTimeWork = "Выходной"}
            };
            context.Schedules.AddRange(schedules);
            context.StatusDepartments.Add(new StatusDepartment { StatusName = "Работает" });
            context.StatusDepartments.Add(new StatusDepartment { StatusName = "Закрыто" });
            context.SaveChanges();


            // установка значений для отделений
            List<string> streets = new List<string> { "Бекетова", "Белгородское шоссе","Белогорская","Береговая","Беркоса","Берута","Бестужева","Библика",
                "Биологическая","Благовещенская","Благодатная","Бобруйская","Богдана Хмельницкого","Боевая","Большая Гончаровская","Большая Жихорская",
                "Большая Кольцевая","Большая Морская","Большая Панасовская","Бориса Чичибабина","Бориса Шрамко","Борьбы","Братская","Броненосца Потемкин",
                "Буковая","Буковинская","бульвар Богдана Хмельницкого","бульвар Грицевца","бульвар Жасминовый","бульвар Ивана Каркача","бульвар Мира",
                "бульвар Нетеченский","бульвар Профсоюзный","бульвар Фронтовиков","бульвар Юрьева","Бурсацкий спуск","Бучмы","Вавилова","Валдайская",
                "Валентиновская","Валерьяновская","Варненская","Василия Мельникова","Василия Стуса","Васильковская","Ватутина","Ващенковский въезд",
                "Велозаводская","Вербовецкого","Веринская","Вернадского","Верхнегиевская","Веселая","Веснина","Виноградная","Витебская","Вишневая",
                "Владимира Сосюры","Владимирская","Владислава Зубенко","Власенко","Военная","Вокзальная","Вологодская","Вологодского","Волонтерская",
                "Волостная","Волховская","Волынская","Воробьева","Воскресенская","Врубеля","Вторчерметовская","Балакирева","Беркоса","Владимира Ридченко",
                "Евгения Плужника","Колхозный","Константина Калинина","Куриловский","Ладыгина","Лебяжий","Магистральный","Марьевский","Михаила Зеленина",
                "Москалевский","Музыкальный","Парковый переулок","Парниковая улица","Паровозная улица","Паровозный переулок","Патона переулок","Паторжинского улица",
                "Патриарха Владимира улица","Патриарха Мстислава Скрипника улица","Патриотов улица","Патриса Лумумбы улица","Паустовского улица","Передовая улица",
                "Переяславская улица","Перова бульвар","Перспективная улица","Петра Болбочана улица","Радистов улица","Радищева улица","Радищева переулок","Радомышльская улица",
                "Радостная улица","Радужная улица","Радунская улица","Раздельная улица","Раисы Букиной улица","Раисы Букиной переулок","Раисы Окипной улица",
                "Райгородская улица","Райгородский переулок","Ракетная улица","Ратманского переулок","Рахманинова улица","Арсенальная площадь","Революции улица",
                "Ревуцкого улица","Регенераторная улица","Редутная улица","Редутный переулок","Резервная улица","Резницкая улица","Рейтарская улица","Ремесленный переулок",
                "Ремонтная улица","Речная улица","Ржевский переулок","Ржищевская улица","Рижская улица","Салютный переулок","Салютный проезд","Самборский переулок",
                "Санаторная улица","Санитарная улица","Сантьяго-де-Чили площадь","Сапёрное Поле улица","Сапёрно-Слободская улица","Сапёрно-Слободской проезд",
                "Буковая","Буковинская","бульвар Богдана Хмельницкого","бульвар Грицевца","бульвар Жасминовый","бульвар Ивана Каркача","бульвар Мира",
                "бульвар Нетеченский","бульвар Профсоюзный","бульвар Фронтовиков","бульвар Юрьева","Бурсацкий спуск","Бучмы","Вавилова","Валдайская",
                "Валентиновская","Валерьяновская","Варненская","Василия Мельникова","Василия Стуса","Васильковская","Ватутина","Ващенковский въезд",
                "Велозаводская","Вербовецкого","Веринская","Вернадского","Верхнегиевская","Веселая","Веснина","Виноградная","Витебская","Вишневая",
                "Владимира Сосюры","Владимирская","Владислава Зубенко","Власенко","Военная","Вокзальная","Вологодская","Вологодского","Волонтерская",
                "Волостная","Волховская","Волынская","Воробьева","Воскресенская","Врубеля","Вторчерметовская","Балакирева","Беркоса","Владимира Ридченко",
                "Евгения Плужника","Колхозный","Константина Калинина","Куриловский","Ладыгина","Лебяжий","Магистральный","Марьевский","Михаила Зеленина",
                "Москалевский","Музыкальный","Парковый переулок","Парниковая улица","Паровозная улица","Паровозный переулок","Патона переулок","Паторжинского улица",
                "Патриарха Владимира улица","Патриарха Мстислава Скрипника улица","Патриотов улица","Патриса Лумумбы улица","Паустовского улица","Передовая улица",
                "Переяславская улица","Перова бульвар","Перспективная улица","Петра Болбочана улица","Радистов улица","Радищева улица","Радищева переулок","Радомышльская улица",
                "Радостная улица","Радужная улица","Радунская улица","Раздельная улица","Раисы Букиной улица","Раисы Букиной переулок","Раисы Окипной улица",
                "Райгородская улица","Райгородский переулок","Ракетная улица","Ратманского переулок","Рахманинова улица","Арсенальная площадь","Революции улица",
                "Ревуцкого улица","Регенераторная улица","Редутная улица","Редутный переулок","Резервная улица","Резницкая улица","Рейтарская улица","Ремесленный переулок",
                "Ремонтная улица","Речная улица","Ржевский переулок","Ржищевская улица","Рижская улица","Салютный переулок","Салютный проезд","Самборский переулок",
                "Санаторная улица","Санитарная улица","Сантьяго-де-Чили площадь","Сапёрное Поле улица","Сапёрно-Слободская улица","Сапёрно-Слободской проезд",
                "Саратовская улица","Свердлова улица","Светлая улица","Светлицкого улица","Светлогорская улица","Свободы проспект","Святославская улица","Бастионная улица",
                "Святошинская площадь","Святошинская улица","Святошинский переулок","Севастопольская площадь","Севастопольская улица","Северная улица","Северо-Сырецкая улица",
                "Седова улица","Седовцев улица","Селекционеров улица","Сельская улица","Сельскохозяйственный переулок","Семафорная улица","Семашко улица","Семёна Палия улица",
                "Семёна Скляренко улица","Семёновская улица","Семьи Издиковских улица","Семьи Сосниных улица","Семьи Стешенко улица","Семьи Хохловых улица",
                "Тараса Шевченко площадь","Тарасовская улица","Таращанская улица","Таращанский переулок","Татарская улица","Татарский переулок","Татьяны Яблонской улица",
                "Ташкентская улица","Тбилисский переулок","Тверской тупик","Театральная площадь","Харьковская площадь","Харьковский переулок","Харьковское шоссе",
                "Хвойная улица","Хмельницкая улица","Холмогорская улица","Холмогорский переулок","Хорива улица","Хорива переулок","Хорольский переулок","Цветущий переулок",
                "Целинная улица","Чешская улица","Чигиринская улица","Чигиринский переулок","Чигорина улица","Электриков улица" };

            int countStreets = streets.Count();
            int[] maxWeight = new int[] { 5, 15, 30, 1000 };
            for (int i = 0; i < countStreets; i++)
            {
                Department department = new Department
                {
                    CityId = random.Next(1, context.Cities.Count() + 1),
                    Adress = streets[0] + ", " + random.Next(1, 150),
                    ScheduleId = random.Next(1,5),
                    MaxWeight = maxWeight[random.Next(4)],
                    StatusDepartmentId = 1
                };
                streets.RemoveAt(0);
                context.Departments.Add(department);
            }

            context.SaveChanges();

            //добавление ролей

            context.Roles.AddRange(new List<Role>
            {
                new Role { RoleName = "admin" },
                new Role { RoleName = "manager" },
                new Role { RoleName = "user" }
            });
            context.SaveChanges();

            UserLogin userLogin1 = new UserLogin
            {
                Email = "admin@gmail.com",
                PasswordHash = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("Rfgh_1245"))),
                RoleId = 1
            };
            UserLogin userLogin2 = new UserLogin
            {
                Email = "manager@gmail.com",
                PasswordHash = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("Rfgh_1245"))),
                RoleId = 2
            };
            UserLogin userLogin3 = new UserLogin
            {
                Email = "user@gmail.com",
                PasswordHash = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("Rfgh_1245"))),
                RoleId = 3
            };
            context.UserLogins.Add(userLogin1);
            context.UserLogins.Add(userLogin2);
            context.UserLogins.Add(userLogin3);
            context.SaveChanges();

            //добавление пользователя отправки
            User user1 = new User
            {
                Id = userLogin1.Id,
                SenderName = "admin",
                FirstName = "admin",
                LastName = "admin",
                ThirdName = "admin",
                PhoneNumber = "0673333333",
                DepartmentId = 1,
            };
            User user2 = new User
            {
                Id = userLogin2.Id,
                SenderName = "manager",
                FirstName = "manager",
                LastName = "manager",
                ThirdName = "manager",
                PhoneNumber = "0502222222",
                DepartmentId = 1,
            };
            User user3 = new User
            {
                Id = userLogin3.Id,
                SenderName = "user, Ltd.",
                FirstName = "user",
                LastName = "user",
                ThirdName = "user",
                PhoneNumber = "0671111111",
                DepartmentId = 119,
            };
            context.Users.Add(user1);
            context.Users.Add(user2);
            context.Users.Add(user3);
            context.SaveChanges();
            
            context.StatusPackages.AddRange(new List<StatusPackage>
            {
                new StatusPackage { StatusName = "В обработке" },
                new StatusPackage { StatusName = "В отделении отправки" },
                new StatusPackage { StatusName = "Отправлен" },
                new StatusPackage { StatusName = "Доставлен в отделение" },
                new StatusPackage { StatusName = "Получен" }
            });
            context.SaveChanges();

            base.Seed(context);
        }
    }
}