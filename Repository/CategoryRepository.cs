// using VirtualMenuAPI.Models;

// namespace VirtualMenuAPI.Repository
// {
//   class CategoryRepository
//   {
//     public List<Category> Categories;
//     public CategoryRepository()
//     {
//       Categories = new(){
//         new(id:0,image:"assets/pizza.jpg",title:"پیتزا"),
//         new(id:1,image:"/assets/sandwich.jpg",title:"برگر"),
//         new(id:2,image:"/assets/tea.jpg",title:"نوشیدنی")
//       };
//     }
//     public List<Category> GetAllCategories()
//     {
//       return Categories;
//     }
//     public Category? GetCategoryById(int id)
//     {
//       return Categories.Find(x => x.Id == id);
//     }
//   }
// }