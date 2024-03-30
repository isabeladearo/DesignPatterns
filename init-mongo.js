db = db.getSiblingDB('Invoices');

db.Contract.insertMany([
  { 
    Description: "Prestação de Serviços Escolares", 
    Amount: 18000.00,
    Periods: 12,
    Date: new Date("2024-03-30T16:33:03.661+00:00"),
    Payments: [
        {
            Amount: 16200.00,
            Date: new Date("2024-03-30T16:33:03.661+00:00")
        }
    ]}
]);
