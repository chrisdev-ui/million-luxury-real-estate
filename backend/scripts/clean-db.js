// Clean MongoDB Database
// Run with: docker exec -i mongodb-milliontest mongosh -u admin -p admin123 --authenticationDatabase admin < clean-db.js

db = db.getSiblingDB('MillionTestDB')

print('🧹 Cleaning MillionTestDB database...')

db.properties.deleteMany({})
db.owners.deleteMany({})
db.propertyImages.deleteMany({})
db.propertyTraces.deleteMany({})

print('✅ Database cleaned!')
print('📊 Current counts:')
print('  - Owners: ' + db.owners.countDocuments())
print('  - Properties: ' + db.properties.countDocuments())
print('  - Images: ' + db.propertyImages.countDocuments())
print('  - Traces: ' + db.propertyTraces.countDocuments())
