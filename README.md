## Environment:
- .NET version: 3.1

## Routes
GET
/contacts
List all contacts

GET
/contacts/{id}
Get a specific contact

POST
/contacts
Create a new contact

PUT
/contacts/{id}
Update a contact

DELETE
/contacts/{id}
Delete a contact

SWAGGER
/swagger

## Data:
SQLITE                                               
Database attached as part of the solution in the ContactAPI project
The name is Contacts.db
For sample purposes the structure is split up in diferent tables in an attempt to match the json format
This design could be improved by denormilizing it to make CRUD operations more simple.

## Sample data:
{
  "id": 20,
  "name": {
    "first": "Harold",
    "middle": "Francis",
    "last": "Gilkey
  },
  "address": {
    "street": "8360 High Autumn Row",
    "city": "Cannon",
    "state": "Delaware",
    "zip": "19797"
  },
  "phone": [
    {
      "number": "302-611-9148",
      "type": "home"
    },
    {
      "number": "302-532-9427",
      "type": "mobile"
    }
  ],                                                                                  
  "email": "harold.gilkey@yahoo.com"
}                                                                                                                                                                                                                                      

## Architecture:                               
Contacts DB API endpoints perform CRUD operations to manage Contacts using SQLITE and entity framework.
                                                                                     