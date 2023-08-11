


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
	- Desserts Delete From DessertCartItems
	- SpecificationId add in DiscountGroup(GetDiscountGroup)
	- Update DessertCartItems

[working on] - Get DiscountGroup select by params DessertId , return different discountGroup suggest
[working on] - Get Desserts by id , Add DessertImageName

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
[working on]課程介紹 API讀取到資料有課程、師資、課程照片 ，前端還沒好
=======0809======
[working on]新增抓取課程分類和單獨抓取課程id ，api都可抓取到資料，但抓課程ID頁面轉換尚未完成

=======0811=======
feat:修改UnitLessonDTO和GetLessonById，新增抓取資料量






==========================================================
Teachers 部分












==========================================================
Orders 部分












==========================================================
Promotions 部分















