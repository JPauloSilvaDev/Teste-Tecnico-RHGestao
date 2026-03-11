using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // IDs fixos para permitir relacionamentos e remoção no Down
            var cust1 = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1");
            var cust2 = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2");

            var prod1 = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1");
            var prod2 = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2");

            var order1 = new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1");

            var orderItem1 = new Guid("dddddddd-dddd-dddd-dddd-ddddddddddd1");
            var orderItem2 = new Guid("dddddddd-dddd-dddd-dddd-ddddddddddd2");

            // Clientes
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Name", "Email" },
                values: new object[,]
                {
                    { cust1, "Empresa Alpha Ltda", "contato@alpha.com" },
                    { cust2, "João da Silva", "joao.silva@example.com" }
                });

            // Produtos
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Description", "Price", "QuantityInStock" },
                values: new object[,]
                {
                    {
                        prod1,
                        "Laptop Dell XPS 15",
                        "Notebook de alto desempenho para uso profissional.",
                        15000.00m,
                        10
                    },
                    {
                        prod2,
                        "Mouse Logitech MX Master",
                        "Mouse sem fio ergonômico com múltiplos botões.",
                        450.00m,
                        25
                    }
                });

            // Pedido de exemplo
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "OrderDate", "ClientId" },
                values: new object[]
                {
                    order1,
                    DateTime.UtcNow,
                    cust1
                });

            // Itens do pedido
            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "OrderId", "ProductId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { orderItem1, order1, prod1, 1, 15000.00m },
                    { orderItem2, order1, prod2, 2,  450.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var cust1 = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1");
            var cust2 = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2");

            var prod1 = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1");
            var prod2 = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2");

            var order1 = new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc1");

            var orderItem1 = new Guid("dddddddd-dddd-dddd-dddd-ddddddddddd1");
            var orderItem2 = new Guid("dddddddd-dddd-dddd-dddd-ddddddddddd2");

            // Apaga itens
            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    orderItem1,
                    orderItem2
                });

            // Apaga pedido
            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: order1);

            // Apaga produtos
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    prod1,
                    prod2
                });

            // Apaga clientes
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    cust1,
                    cust2
                });
        }
    }
}
