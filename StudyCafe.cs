using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace StudyCafeManager
{
    [Serializable]
    class StudyCafe
    {
        private Dictionary<string, Person> users;
        private Dictionary<ISeat, Person> bookStatus;
        private List<ISeat> seat;
        private Admin admin;
        private string select;
        Person person;
      
        public StudyCafe()
        {
            admin = new Admin();
            users = new Dictionary<string, Person>();
            bookStatus = new Dictionary<ISeat, Person>();
            seat = new List<ISeat>();
            seat = CreateSeats();
            Load();
        }

        #region<Menu part>
        public void Menu()
        {

            bool Menu = true;
            while (Menu)
            {
                Console.Clear();
                Console.WriteLine("************************************************");
                Console.WriteLine("************************************************");
                Console.WriteLine("*              Ecount Study Lounge             *");
                Console.WriteLine("************************************************");
                Console.WriteLine("************************************************");
                Console.WriteLine("*1. 로그인         2. 가입         3. 종료     *");
                Console.WriteLine("************************************************");
                Notice();
                Console.WriteLine();
                Console.Write("원하시는 메뉴를 선택해주세요. : ");
                select = Console.ReadLine();
                switch (select)
                {
                    case "1":
                        person = login();
                        if (person == null)
                        {
                            Console.WriteLine("이메일, 비밀번호를 잘못 입력했습니다. 엔터를 누르면 돌아갑니다.");
                            Console.ReadLine();
                            break;
                        }
                        else
                        {
                            MainMenu(person);
                        }                      
                        break;
                    case "2":
                        AddUser();
                        break;
                    case "3":
                        Menu = false;
                        break;
                    default:
                        Console.WriteLine("1,2,3만 입력해주세요. 엔터를 누르면 돌아갑니다.");
                        Console.ReadLine();                        
                        break;
                }
            }          
        }

        public void MainMenu(Person person)
        {
            
            string loginType = Convert.ToString(person.Name);
            bool mainMenu = true;
            while (mainMenu)
            {
                if (loginType == "admin")
                {
                    Console.Clear();
                    Console.WriteLine("************************************************************");
                    Console.WriteLine("*                       관리자 모드                        *");
                    Console.WriteLine("************************************************************");
                    Console.WriteLine("*1. 회원조회  2. 예약현황  3. 예약금지 관리  4. 돌아가기   *");
                    Console.WriteLine("************************************************************");
                    Console.Write("원하시는 메뉴를 선택해주세요. : ");
                    select = Console.ReadLine();
                    switch (select)
                    {
                        case "1":
                            CheckAllUser();
                            break;
                        case "2":
                            CheckAllBook();
                            Console.WriteLine("엔터키를 입력하면 메인으로 돌아갑니다. ");
                            Console.ReadLine();
                            break;
                        case "3":
                            Block();
                            break;
                        case "4":
                            mainMenu = false;
                            person = null;
                            break;
                        default:
                            Console.WriteLine("1,2,3,4 만 입력해주세요. 엔터를 누르면 돌아갑니다.");
                            Console.ReadLine();
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("*********************************************************************************");
                    Console.WriteLine("*                             Ecount Study Lounge                               *");
                    Console.WriteLine("*********************************************************************************");
                    Console.WriteLine("*1. 예약하기   2. 예약변경  3. 예약취소  4. 예약조회  5. 회원탈퇴  6. 돌아가기  *");
                    Console.WriteLine("*********************************************************************************");
                    Console.Write("원하시는 메뉴를 선택해주세요. : ");
                    select = Console.ReadLine();
                    switch (select)
                    {
                        case "1":
                            Book();
                            break;
                        case "2":
                            Change();
                            break;
                        case "3":
                            CancleBook();
                            break;
                        case "4":
                            CheckBook();
                            break;
                        case "5":
                            DelUser();
                            person = null;
                            mainMenu = false;
                            break;
                        case "6":
                            mainMenu = false;
                            break;
                        default:
                            Console.WriteLine("1,2,3,4,5,6 만 입력해주세요. 엔터를 누르면 돌아갑니다.");
                            Console.ReadLine();
                            break;
                    }
                }
            }
        }
        #endregion
        #region<User Method>
        public void AddUser()
        {
            Console.Clear();
            bool start = true;
            string name = null;
            string email = null;
            while (start)
            {
                
                Console.Write($"\n이름 : ");
                name = Console.ReadLine();
                Console.Write($"\n이메일 : ");
                email = Console.ReadLine();

                if (Check_email(email))
                {
                    start = false;
                }
                else
                {
                    continue;
                }
                
                if(users.ContainsKey(email))
                {
                    Console.WriteLine("기존에 있는 이메일입니다. 다시 입력해주세요");
                    Console.ReadLine();
                    start = true;
                    continue;
                }
            }
            Console.Write($"password : ");
            string pw = Console.ReadLine();

            users.Add(email, new User(name, email, pw));
            Save_user();
            Console.WriteLine("회원가입이 완료되었습니다. 엔터를 입력하면 메인으로 돌아갑니다.");
            Console.ReadLine();
        }

        public bool Check_email(string email)
        {
            Regex regex = new Regex(@"^([0-9a-zA-Z]+){3,}@([0-9a-zA-Z]+){3,}(\.[0-9a-zA-Z]+){1,}$");
            bool check = regex.IsMatch(email);

            if (check != true)
            {
                Console.WriteLine("이메일을 잘못 입력하셨습니다. 다시 입력해주세요");
                return false;
            }
            else return true;

        }
        public void DelUser()
        {
            Console.Clear();
            Console.WriteLine("회원을 정말 탈퇴하시겠습니까? 숫자 1과 엔터를 입력하면 삭제합니다. 삭제를 원치 않으시면 아무키나 누르고 엔터를 입력해주세요. ");
            string chooseDel = Console.ReadLine();

            if (chooseDel == "1")
            {
                foreach (var item in bookStatus)
                {
                    if (item.Value.Email == person.Email)
                    {
                        seat[seat.FindIndex(x => x.SeatNum == item.Key.SeatNum)].Status = "예약가능";
                        bookStatus.Remove(item.Key); //키값지우기
                        users.Remove(person.Email);
                        break;
                    }
                }
            }
            else
            {
                return;
            }
            Save_user();
            Save_bookstatus();
            Save_seat();
            person = null;
            Console.WriteLine("회원탈퇴가 완료되었습니다. 엔터키를 누르면 메인으로 돌아갑니다.");
            Console.ReadLine();
        }
        public void CheckBook()  //user 가 자신의 예약내역을 확인한다.
        {
            Console.Clear();
            foreach (var item in bookStatus)
            {
                if (item.Value.Email == person.Email)
                {
                    Console.WriteLine($"예약하신 좌석은{item.Key.SeatNum}입니다. 엔터를 누르면 돌아갑니다.");
                    Console.ReadLine();
                    return;
                }
            }
            Console.WriteLine("예약하신 좌석이 없습니다. 엔터를 누르면 메인으로 돌아갑니다.");
            Console.ReadLine();
        }
        public void Book()
        {
            // 민성
            Console.Clear();

            // 좌석현황 보여주기 
            ShowAllSeat();
            foreach (var item in bookStatus)
            {
                if (item.Value.Email == person.Email)
                {
                    Console.WriteLine("이미 예약하신 좌석이 있습니다. 엔터키를 입력하면 메인으로 돌아갑니다.");
                    Console.ReadLine();
                    return;
                }
            }
            Console.WriteLine("좌석을 선택해 주세요");

            string input = Console.ReadLine();
            bool select = true;

            while (select)
            {

                if (seat.Exists(x => x.SeatNum == input) && seat[seat.FindIndex(x => x.SeatNum == input)].Status == "예약가능") // 정의해야 함 (예약가능,예약됨,금지)
                {
                    Console.WriteLine("현재 좌석은 예약이 안되었습니다 예약하시겠습니까? 1. 예약하기 2. 뒤로가기");
                    string input2 = Console.ReadLine();

                    if (input2 == "1")
                    {
                        bookStatus.Add(seat[seat.FindIndex(x => x.SeatNum == input)], person);
                        seat[seat.FindIndex(x => x.SeatNum == input)].Status = "예약완료";
                        Save_bookstatus();
                        Save_seat();
                        select = false;
                        break;
                    }
                    else if (input2 == "2")
                    {
                        Console.WriteLine(input2 == "2");
                        select = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("1. 예약하기 또는 2. 뒤로가기를 입력해주세요.");
                        Console.ReadLine();
                    }                   
                }
                else
                {
                    Console.WriteLine("예약 불가능한 좌석입니다. 엔터를 누르면 돌아갑니다.");
                    Console.ReadLine();
                    select = false;
                }
            }
        }
        public void Change()
        {
            Console.Clear();
            Console.WriteLine("좌석을 변경하시겠습니까? 1.네 2. 아니오 (2번 선택시 메인으로 돌아갑니다");
            select = Console.ReadLine();
            if (select == "1")
            {
                //private List<ISeat> seat
                //자기예약한좌석 불러와서 예약없애고 새로등록
                ShowAllSeat();
                string nowSeatNum = null;
                foreach (var item in bookStatus)
                {
                    if (item.Value.Email == person.Email)
                    {
                        nowSeatNum = item.Key.SeatNum;
                        break;
                    }
                }
                Console.Write($"{person.Name}님이 현재 예약하신 좌석은 {nowSeatNum}입니다. 어느 좌석으로 변경하시겠습니까? 좌석을 입력해주세요 : ");
                string input = Console.ReadLine();  //방이 맞는지 안맞는지 - > 13개 seatnum 중에 내가 입력한걸 체크해서
                bool seatnumT = seat.Exists(x => x.SeatNum == input && x.Status == "예약가능");
                if (seatnumT == true)
                {
                    Console.WriteLine("예약 가능한 좌석입니다. 예약하시겠습니까? 1.예약 2.아니오");
                    string choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        ISeat key = bookStatus.FirstOrDefault(x => x.Value.Email == person.Email).Key;
                        seat[seat.FindIndex(x => x.SeatNum == key.SeatNum)].Status = "예약가능";
                        bookStatus.Remove(key); //키값지우기
                        bookStatus.Add(seat[seat.FindIndex(x => x.SeatNum == input)], person);
                        seat[seat.FindIndex(x => x.SeatNum == input)].Status = "예약완료";
                        Save_bookstatus();
                        Save_seat();
                        Console.WriteLine("예약 변경이 완료되었습니다. 엔터키를 입력하면 메인으로 돌아갑니다.");
                        Console.ReadLine();
                        return;                        
                    }
                    else
                    {
                        return;
                    }                                     
                }
                else
                {
                    Console.WriteLine("선택하신 좌석은 예약이 불가능 합니다. 엔터키를 입력하면 메인으로 돌아갑니다.");
                    Console.ReadLine();
                }
            }
            else if (select == "2")
            {
                return;
            }
            else
            {
                Console.WriteLine("잘못입력하셨습니다. 1,2만 입력해주세요");
                return;
            }

            Console.WriteLine("예약한 좌석이 없습니다.");
        }

        public void CancleBook()
        {
            Console.Clear();
            foreach (var item in bookStatus)
            {
                if (item.Value.Email == person.Email)
                {
                    Console.WriteLine($"예약하신 좌석은{item.Key.SeatNum}입니다. 예약을 취소하시겠습니까? 1 : 취소, 2 : 돌아가기");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            seat[seat.FindIndex(x => x.SeatNum == item.Key.SeatNum)].Status = "예약가능";
                            bookStatus.Remove(item.Key); //키값지우기
                            Save_bookstatus();
                            Save_seat();
                            return;
                        case "2":
                            break;
                        default:
                            Console.WriteLine("1 또는 2만 입력가능합니다.");
                            break;
                    }
                }
            }
            Console.WriteLine("예약 취소가 완료되었습니다. 엔터키를 입력하면 메인으로 돌아갑니다.");
            Console.ReadLine();
        }
        #endregion
        #region<Admin Method>
        public void CheckAllUser()
        {
            Console.Clear();
            Console.WriteLine("전체 회원 현황");

            foreach (var item in users)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("엔터를 입력하면 메뉴로 돌아갑니다.");
            Console.ReadLine();
        }
        public void CheckAllBook()
        {
            Console.WriteLine("현재 전체 예약현황입니다.");

            foreach (var item in bookStatus)
            {
                Console.WriteLine($"좌석번호 : {item.Key.SeatNum}, 예약자명 : {item.Value.Name}, 이메일 : {item.Value.Email}");               
            }           
        }
        public void Block()
        {
            Console.Clear();

            ShowAllSeat();
            CheckAllBook();
            if (true)   // 금지하기
            {
                Console.WriteLine("금지할 좌석을 선택해 주세요");

                string input = Console.ReadLine();

                if (seat.Exists(x => x.SeatNum == input) && seat[seat.FindIndex(x => x.SeatNum == input)].Status == "예약가능") // 정의해야 함 (예약가능,예약됨,금지)
                {
                    Console.WriteLine("현재 좌석을 금지 하시겠습니까? 1. 금지하기 2. 뒤로가기");
                    string choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        seat[seat.FindIndex(x => x.SeatNum == input)].Status = "예약금지";
                        Save_seat();
                        Console.WriteLine("예약 금지 지정이 완료되었습니다. 엔터키를 입력하면 메인으로 돌아갑니다.");
                        Console.ReadLine();
                    }
                    else if (choice == "2")
                    {
                        Console.WriteLine("관리자 메뉴로 돌아갑니다. 엔터키를 입력하면 메인으로 돌아갑니다.");
                        Console.ReadLine();
                    }
                    return;
                }
                else
                {
                    Console.WriteLine("해당 좌석은 금지가 불가능합니다. 엔터키를 입력하면 메인으로 돌아갑니다.");
                    Console.ReadLine();
                }
            }
            else                // 금지풀기
            {

            }               
        }
        public void ShowAllBook()
        {
            Console.Clear();
            Console.WriteLine("현재 좌석 현황입니다.");
            foreach (var key in bookStatus) //iseat만 보여줌
            {
                Console.WriteLine(key);
            }
        }
        #endregion
        #region<Common and Private Method>
        private void Load()
        {
            string user_path = $@"C:\StudyCafeTest\user.txt";
            string admin_path = $@"C:\StudyCafeTest\admin.txt";
            string bookstatus_path = $@"C:\StudyCafeTest\" + $"{DateTime.Now.ToString("yy.MM.dd")}_bookstatus.txt";
            string seat_path = $@"C:\StudyCafeTest\seat.txt";

            // user 로드
            if (File.Exists(user_path))
            {
                using (Stream user_open = new FileStream(user_path, FileMode.OpenOrCreate))
                {
                    BinaryFormatter user_bf = new BinaryFormatter();
                    users = (Dictionary<string, Person>)user_bf.Deserialize(user_open);

                }
            }
            //book_status 로드           
            if (File.Exists(bookstatus_path))
            {
                using (Stream bookstatus_open = new FileStream(bookstatus_path, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bookstatus_bf = new BinaryFormatter();
                    bookStatus = (Dictionary<ISeat, Person>)bookstatus_bf.Deserialize(bookstatus_open);
                }
            }            
            //seat 로드
            if (!File.Exists(seat_path))
            {
                Save_seat();
            }           
            else
            {
                using (Stream seat_open = new FileStream(seat_path, FileMode.OpenOrCreate))
                {
                    BinaryFormatter seat_bf = new BinaryFormatter();            
                    seat = (List<ISeat>)seat_bf.Deserialize(seat_open);
                }
            }
        }
        private void Save_user()
        {
            string user_path = $@"C:\StudyCafeTest\user.txt";

            using (Stream user_save = new FileStream(user_path, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(user_save, users);
            }

        }

        private void Save_bookstatus()
        {
            string bookstatus_path = $@"C:\StudyCafeTest\" + $"{DateTime.Now.ToString("yy.MM.dd")}_bookstatus.txt";

            using (Stream bookstatus_save = new FileStream(bookstatus_path, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(bookstatus_save, bookStatus);
            }

        }

        private void Save_seat()
        {
            string seat_path = $@"C:\StudyCafeTest\seat.txt";

            using (Stream seat_save = new FileStream(seat_path, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(seat_save, seat);
            }

        }
        private void Notice()
        {
            // 민성
            int count = 0;

            foreach (var item in seat)
            {
                if (item.Status == "예약금지" || item.Status == "예약완료")
                {
                    count++;
                }
            }
            Console.WriteLine($"전체 이용 가능 좌석 수는 :  {seat.Count} , 예약 가능한 좌석 수는 : {seat.Count - count} , 예약 불가능 한 좌석 수는  : {count}");
        }
        private void ShowAllSeat()
        {
            Console.WriteLine("--------------개인좌석--------------");
            foreach (var item in seat)
            {
                if (item.SeatNum.Contains("P"))
                {
                    Console.Write($"[{item.SeatNum}] : [{item.Status}]    ");
                }
            }
            Console.WriteLine();
            Console.WriteLine("--------------스터디룸--------------");
            foreach (var item in seat)
            {
                if (item.SeatNum.Contains("R"))
                {
                    Console.Write($"[{item.SeatNum}] : [{item.Status}]    ");
                }
            }
            Console.WriteLine();
        }
        public Person login()
        {
            Console.Clear();
            Console.Write("이메일 주소를 입력해주세요. : ");
            string email = Console.ReadLine();
            Console.Write("비밀번호를 입력해주세요. : ");
            string pw = Console.ReadLine();

            if (email != "admin")
            {
                foreach (User item in users.Values)
                {
                    if (item.Email == email && item.PW == pw)
                    {
                        person = item;
                        return person;
                    }
                }
            }
            else
            {
                if (pw == admin.PW)
                {
                    person = admin;
                }
            }
            return person;
        }
        private List<ISeat> CreateSeats()
        {
            List<ISeat> list = new List<ISeat>();

            for(int i = 0; i < 10; i++)
            {
                list.Add(new PrivateSeat($"P-{i+1}"));
            }
            for (int i = 0; i < 3; i++)
            {
                list.Add(new Room($"R-{i+1}"));
            }
            foreach (var item in list)
            {
                item.Status = "예약가능";
            }
            return list;
        }
        #endregion
    }
}
