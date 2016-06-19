备注：
MyLib/Oracle.DataAccess.dll为64位，所以使用的客户端程必须运行为64位的才行。


关于“TransactionScope”
同一台机不用ip会使用MSDTC，同ip同实例不同数据会使用MSDTC（貌似connectionString是一模一样，name可以不一样）。

相关开源框架：
Dos.ORM
SqlFu

