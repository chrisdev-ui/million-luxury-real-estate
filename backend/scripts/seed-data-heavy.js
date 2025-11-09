// MongoDB Heavy Seed Data Script - Performance Testing
// Run with: docker exec -i mongodb-milliontest mongosh -u admin -p admin123 --authenticationDatabase admin < seed-data-heavy.js

db = db.getSiblingDB('MillionTestDB')

print('🧹 Cleaning existing data...')
db.properties.deleteMany({})
db.owners.deleteMany({})
db.propertyImages.deleteMany({})
db.propertyTraces.deleteMany({})

const cities = ['Miami', 'New York', 'Los Angeles', 'Chicago', 'Houston', 'Phoenix', 'Philadelphia', 'San Antonio', 'San Diego', 'Dallas']
const streets = ['Main St', 'Ocean Dr', 'Park Ave', 'Broadway', 'Sunset Blvd', 'Beach Rd', 'Downtown Ave', 'Harbor View', 'Skyline Dr', 'Palm Blvd']
const propertyTypes = ['Luxury Villa', 'Modern Apartment', 'Penthouse', 'Beach House', 'Condo', 'Townhouse', 'Mansion', 'Studio', 'Duplex', 'Loft']
const firstNames = ['John', 'Jane', 'Michael', 'Sarah', 'David', 'Emily', 'Robert', 'Lisa', 'James', 'Maria']
const lastNames = ['Smith', 'Johnson', 'Williams', 'Brown', 'Jones', 'Garcia', 'Miller', 'Davis', 'Rodriguez', 'Martinez']

print('👥 Creating 50 owners...')
const owners = []
for (let i = 0; i < 50; i++) {
  const firstName = firstNames[Math.floor(Math.random() * firstNames.length)]
  const lastName = lastNames[Math.floor(Math.random() * lastNames.length)]
  const city = cities[Math.floor(Math.random() * cities.length)]

  const owner = db.owners.insertOne({
    name: `${firstName} ${lastName}`,
    address: `${Math.floor(Math.random() * 9999) + 100} ${streets[Math.floor(Math.random() * streets.length)]}, ${city}`,
    photo: `https://images.unsplash.com/photo-${1560250097 + i}0-0b93528c311a?w=200`,
    birthday: new Date(1950 + Math.floor(Math.random() * 50), Math.floor(Math.random() * 12), Math.floor(Math.random() * 28) + 1),
    createdAt: new Date(),
    updatedAt: new Date()
  })
  owners.push(owner.insertedId)
}

print('🏠 Creating 500 properties...')
const properties = []
const imageUrls = [
  'https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=800',
  'https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=800',
  'https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=800',
  'https://images.unsplash.com/photo-1580587771525-78b9dba3b914?w=800',
  'https://images.unsplash.com/photo-1518780664697-55e3ad937233?w=800',
  'https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800',
  'https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800',
  'https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800'
]

for (let i = 0; i < 500; i++) {
  const propType = propertyTypes[Math.floor(Math.random() * propertyTypes.length)]
  const city = cities[Math.floor(Math.random() * cities.length)]
  const street = streets[Math.floor(Math.random() * streets.length)]
  const ownerId = owners[Math.floor(Math.random() * owners.length)]
  const basePrice = 300000 + Math.floor(Math.random() * 5000000)

  const prop = db.properties.insertOne({
    name: `${propType} ${city} #${i + 1}`,
    address: `${Math.floor(Math.random() * 9999) + 100} ${street}, ${city}, FL ${33000 + Math.floor(Math.random() * 500)}`,
    price: NumberDecimal(basePrice.toString()),
    codeInternal: `${city.substring(0, 3).toUpperCase()}-${String(i + 1).padStart(4, '0')}`,
    year: 2000 + Math.floor(Math.random() * 25),
    idOwner: ownerId,
    enabled: Math.random() > 0.1,
    createdAt: new Date(Date.now() - Math.floor(Math.random() * 365 * 24 * 60 * 60 * 1000)),
    updatedAt: new Date()
  })
  properties.push({ id: prop.insertedId, price: basePrice })
}

print('🖼️ Creating 1500 property images (3 per property average)...')
const images = []
for (let i = 0; i < 500; i++) {
  const numImages = 2 + Math.floor(Math.random() * 4)
  for (let j = 0; j < numImages; j++) {
    images.push({
      idProperty: properties[i].id.toString(),
      file: imageUrls[Math.floor(Math.random() * imageUrls.length)],
      enabled: true,
      createdAt: new Date()
    })
  }
}
db.propertyImages.insertMany(images)

print('📊 Creating 2000 property traces (sales history)...')
const traceNames = ['Initial Sale', 'Purchase', 'Resale', 'Investment Sale', 'Market Sale', 'Transfer', 'Refinance Sale']
const traces = []
for (let i = 0; i < 500; i++) {
  const numTraces = 1 + Math.floor(Math.random() * 5)
  let currentPrice = properties[i].price * 0.7

  for (let j = 0; j < numTraces; j++) {
    const saleDate = new Date(Date.now() - (numTraces - j) * 180 * 24 * 60 * 60 * 1000)
    currentPrice = currentPrice * (1 + (Math.random() * 0.3))

    traces.push({
      idProperty: properties[i].id.toString(),
      dateSale: saleDate,
      name: traceNames[Math.floor(Math.random() * traceNames.length)],
      value: NumberDecimal(Math.floor(currentPrice).toString()),
      tax: NumberDecimal(Math.floor(currentPrice * 0.05).toString()),
      createdAt: new Date()
    })
  }
}
db.propertyTraces.insertMany(traces)

print('\n✅ Heavy seed data inserted successfully!')
print('📊 Summary:')
print('  - Owners: ' + db.owners.countDocuments())
print('  - Properties: ' + db.properties.countDocuments())
print('  - Images: ' + db.propertyImages.countDocuments())
print('  - Traces: ' + db.propertyTraces.countDocuments())
print('\n🔍 Sample queries for testing:')
print('  - Properties with "Villa": ' + db.properties.countDocuments({name: /Villa/i}))
print('  - Properties in Miami: ' + db.properties.countDocuments({address: /Miami/i}))
print('  - Properties $500k-$1M: ' + db.properties.countDocuments({price: {$gte: NumberDecimal("500000"), $lte: NumberDecimal("1000000")}}))
