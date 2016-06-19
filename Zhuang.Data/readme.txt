备注：
MyLib/Oracle.DataAccess.dll为64位，所以使用的客户端程必须运行为64位的才行。


关于“TransactionScope”
同一台机不用ip会使用MSDTC，同ip同实例不同数据会使用MSDTC（貌似可以是不同的两个配置，但connectionString要一模一样，有空格都不行）。

相关开源框架：
Dos.ORM
SqlFu

