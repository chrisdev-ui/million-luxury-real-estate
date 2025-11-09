// MongoDB Seed Data Script
// Run with: docker exec -it mongodb-milliontest mongosh < seed-data.js

db = db.getSiblingDB('MillionTestDB')

// Clear existing data
db.properties.deleteMany({})
db.owners.deleteMany({})
db.propertyImages.deleteMany({})
db.propertyTraces.deleteMany({})

// Insert Owners
const owner1 = db.owners.insertOne({
  name: 'John Doe',
  address: '123 Main St, Miami, FL',
  photo: 'https://images.unsplash.com/photo-1560250097-0b93528c311a?w=200',
  birthday: new Date('1980-01-15'),
  createdAt: new Date(),
  updatedAt: new Date()
})

const owner2 = db.owners.insertOne({
  name: 'Jane Smith',
  address: '456 Park Ave, New York, NY',
  photo: 'https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=200',
  birthday: new Date('1985-05-20'),
  createdAt: new Date(),
  updatedAt: new Date()
})

const ownerId1 = owner1.insertedId
const ownerId2 = owner2.insertedId

// Insert Properties
const prop1 = db.properties.insertOne({
  name: 'Luxury Villa Miami Beach',
  address: '456 Ocean Drive, Miami Beach, FL 33139',
  price: NumberDecimal('2500000'),
  codeInternal: 'MIA-001',
  year: 2020,
  idOwner: ownerId1,
  enabled: true,
  createdAt: new Date(),
  updatedAt: new Date()
})

const prop2 = db.properties.insertOne({
  name: 'Modern Apartment Downtown',
  address: '789 Brickell Ave, Miami, FL 33131',
  price: NumberDecimal('850000'),
  codeInternal: 'MIA-002',
  year: 2019,
  idOwner: ownerId1,
  enabled: true,
  createdAt: new Date(),
  updatedAt: new Date()
})

const prop3 = db.properties.insertOne({
  name: 'Beach House Key Biscayne',
  address: '321 Crandon Blvd, Key Biscayne, FL 33149',
  price: NumberDecimal('3200000'),
  codeInternal: 'KEY-001',
  year: 2021,
  idOwner: ownerId2,
  enabled: true,
  createdAt: new Date(),
  updatedAt: new Date()
})

const prop4 = db.properties.insertOne({
  name: 'Penthouse Coconut Grove',
  address: '555 Bayshore Dr, Coconut Grove, FL 33133',
  price: NumberDecimal('1750000'),
  codeInternal: 'COC-001',
  year: 2022,
  idOwner: ownerId2,
  enabled: true,
  createdAt: new Date(),
  updatedAt: new Date()
})

const prop5 = db.properties.insertOne({
  name: 'Condo South Beach',
  address: '888 Collins Ave, Miami Beach, FL 33139',
  price: NumberDecimal('650000'),
  codeInternal: 'SBE-001',
  year: 2018,
  idOwner: ownerId1,
  enabled: true,
  createdAt: new Date(),
  updatedAt: new Date()
})

// Insert Property Images
db.propertyImages.insertMany([
  {
    idProperty: prop1.insertedId.toString(),
    file: 'https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=800',
    enabled: true,
    createdAt: new Date()
  },
  {
    idProperty: prop1.insertedId.toString(),
    file: 'https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=800',
    enabled: true,
    createdAt: new Date()
  },
  {
    idProperty: prop2.insertedId.toString(),
    file: 'https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=800',
    enabled: true,
    createdAt: new Date()
  },
  {
    idProperty: prop3.insertedId.toString(),
    file: 'https://images.unsplash.com/photo-1580587771525-78b9dba3b914?w=800',
    enabled: true,
    createdAt: new Date()
  },
  {
    idProperty: prop4.insertedId.toString(),
    file: 'https://images.unsplash.com/photo-1518780664697-55e3ad937233?w=800',
    enabled: true,
    createdAt: new Date()
  },
  {
    idProperty: prop5.insertedId.toString(),
    file: 'https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800',
    enabled: true,
    createdAt: new Date()
  }
])

// Insert Property Traces (Sales History)
db.propertyTraces.insertMany([
  {
    idProperty: prop1.insertedId.toString(),
    dateSale: new Date('2023-06-15'),
    name: 'Initial Sale',
    value: NumberDecimal('2500000'),
    tax: NumberDecimal('125000'),
    createdAt: new Date()
  },
  {
    idProperty: prop2.insertedId.toString(),
    dateSale: new Date('2023-03-20'),
    name: 'Purchase',
    value: NumberDecimal('850000'),
    tax: NumberDecimal('42500'),
    createdAt: new Date()
  }
])

print('✅ Seed data inserted successfully!')
print('📊 Summary:')
print('  - Owners: ' + db.owners.countDocuments())
print('  - Properties: ' + db.properties.countDocuments())
print('  - Images: ' + db.propertyImages.countDocuments())
print('  - Traces: ' + db.propertyTraces.countDocuments())
