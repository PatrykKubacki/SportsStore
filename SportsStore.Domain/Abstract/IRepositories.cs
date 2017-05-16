using System.Collections.Generic;
using SportsStore.Domain.Data;

namespace SportsStore.Domain.Abstract
{

    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }
        void SaveCategory(Category category);
        Category DeleteCategory(int Id);
    }


    public interface ICityRepository
    {
        IEnumerable<City> Cities { get; }
        void SaveCity(City city);
        City DeleteCity(int Id);
    }


    public interface IOrderRepository
    {
        IEnumerable<Order> Orders { get; }
    }


    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
        IEnumerable<Category> Categories { get; }
        void SaveProduct(Product product);
        Product DeleteProduct(int Id);
    }


    public interface IRoleRepository
    {
        IEnumerable<Role> Roles { get; }
        void SaveRole(Role role);
        Role DeleteRole(int Id);
    }


    public interface IUserRepository
    {
        IEnumerable<User> Users { get; }
        IEnumerable<Address> Addresses { get; }
        IEnumerable<Role> Roles { get; }
        void SaveUser(User user);
        User DeleteUser(int Id);
        void ConfirmEmail(string email);
        void ChangePassword(int Id, string password);
        string ResetPassword(int Id);
    }


    public interface IAddressRepository
    {
        IEnumerable<Address> Addresses { get; }
        IEnumerable<City> Cities { get; }
        void SaveAddress(Address address);
        Address DeleteAddress(int Id);
    }

}
