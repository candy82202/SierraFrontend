


=========================================================
前台頁面 部分





=========================================================
Desserts 部分
[index]
[V] - Get Top Sales 3 Desserts

[desserts]
[V] - Get all categories desserts to views
[V] - Turn into Three Layers
	- Add new IDessertRepository
		- method Getpresents , GetMoldCake , GetLongCake , GetSnack , GetRoomTemperature ,TopSaleDesserts
	- Add new IDessertCategoryRepository
		- Select desserts base on different categoryId 

[dessert discount group]		
[V] - Get desserts in discount group (Except catagories) use dapper
	- IDessertDiscountRepository GetDiscountGroupsByGroupId method
	- DessertDiscountDPRepository interface  IDessertDiscountRepository
	- DessertService call for different DiscountGroupId , To get different method
	- Get DiscountGroup select by params DessertId , return different discountGroup suggest
	- Get Desserts by id , Add DessertImageName

[cart system]
[V] - Desserts Add To Cart 
	- Desserts Delete From DessertCartItems
	- SpecificationId add in DiscountGroup(GetDiscountGroup)
	- Update DessertCartItems

[customer service]
[V] - Open Ai API For Customer Service
	- IOpenAiService , OpenAiService ,OpenAiController
	- Complete sentences
	- Chat (answer question like customer service)
	- Add Dessert , Categories ,DessertDiscountGroup , MemberCoupon

[V] - Unit Test
=========================================================
Members 部分
[V] - Login(with JWT Authentication & thress-layer Architecture)
	- Modify Programs.cs 
[V] - Register
[V] - EmailHelper.cs
[V] - ForgotPassword
[V] - Modify EmailHelper.cs
[V] - GoogleLogin
[V] - EditPassword
[V] - GetMember & EditMember
[V] - EditMemberImage & GetMemberImageUrl
[V] - Modify MemberService.EditPassword() logic
[V] - Rename ForgotPassword & ResetPassword
[V] - Dapper
[V] - Modify ErrorMessage
[V] - Modify "127.0.0.1" to "localhost" 
[V] - Add password rules
[V] - Modify GoogleRegister







==========================================================
Lessons 部分

=======0807======
[V]師資介紹
[working on]課程介紹 API讀取到資料有課程、師資、課程照片 ，前端還沒好
=======0809======
[working on]新增抓取課程分類和單獨抓取課程id ，api都可抓取到資料，但抓課程ID頁面轉換尚未完成

=======0811=======
feat:修改UnitLessonDTO和GetLessonById，新增抓取資料量


=======0822=========
feat:lessons改為三層式寫法



==========================================================
Teachers 部分












==========================================================
Orders 部分












==========================================================
Promotions 部分















