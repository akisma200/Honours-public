import csv
import json

csvfile = open('household_power_consumption_one_year.csv', 'r')
jsonfile = open('household_power_consumption_one_year.json', 'w')

fieldnames = ("Date","Time","Global_active_power","Global_reactive_power","Voltage","Global_intensity","Sub_metering_1","Sub_metering_2","Sub_metering_3")
reader = csv.DictReader( csvfile, fieldnames)
for row in reader:
    json.dump(row, jsonfile)
    jsonfile.write(',\n')