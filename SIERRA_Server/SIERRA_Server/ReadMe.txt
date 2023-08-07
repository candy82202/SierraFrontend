


=========================================================
前台頁面 部分





=========================================================
Desserts 部分
[V] - Get all categories desserts to views
[V] - Get Top Sales 3 Desserts
[V] - Turn into Three Layers
	- Add new IDessertRepository
		- method Getpresents , GetMoldCake , GetLongCake , GetSnack , GetRoomTemperature ,TopSaleDesserts
	- Add new IDessertCategoryRepository
		- Select desserts base on different categoryId 
		
[V] - Get desserts in discount group (Except catagories)
	- IDessertDiscountRepository GetDiscountGroupsByGroupId method
	- DessertDiscountDPRepository interface  IDessertDiscountRepository
	- DessertService call for different DiscountGroupId , To get different method


[V] - Desserts Add To Cart 

[working on] - If DessertId Exist In Cart , then Quantity Plus 1

=========================================================
Members 部分
[V] - Login(with JWT Authentication & thress-layer Architecture)
	- Modify Programs.cs 
[V] - Register
[V] - EmailHelper.cs










==========================================================
Lessons 部分

=======0807======
[V]師資介紹
[working on]課程介紹









==========================================================
Teachers 部分












==========================================================
Orders 部分












==========================================================
Promotions 部分















