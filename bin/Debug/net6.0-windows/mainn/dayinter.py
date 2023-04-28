import pandas as pd
from statsmodels.graphics.tsaplots import plot_acf
import re

class dayinter():
    ''' This class is used to interpolate the missing values in the time series data.
     The data is interpolated using the linear interpolation method.
     The class takes two arguments: date and values.'''
    def get_date_format(self, date):
        '''This function is used to get the date format of the date column.'''
        if re.match(r"^\d{4}-\d{2}-\d{2}$", date):
            return "%Y-%m-%d"
        elif re.match(r"^\d{2}-\d{2}-\d{4}$", date):
            return "%d-%m-%Y"
        elif re.match(r"^\d{2}/\d{2}/\d{4}$", date):
            return "%m/%d/%Y"
        elif re.match(r"^\d{4}/\d{2}/\d{2}$", date):
            return "%Y/%d/%m"
        elif re.match(r"^\d{4}\d{2}\d{2}$", date):
            return "%Y%m%d"
        elif re.match(r"^\d{2}\d{2}\d{4}$", date):
            return "%d%m%Y"
        elif re.match(r"^\d{4}/\d{2}/\d{4}$", date):
            return "%Y/%m/%d"
        elif re.match(r"^\d{2} \w{3} \d{4}$", date):
            return "%d %b %Y"
        elif re.match(r"^\d{2} \w{4,9} \d{4}$", date):
            return "%d %B %Y"
        else:
            return None
    
    def __init__(self, data1, data2):
        fdata1 = data1.split("|")[0]
        rx = data1.split("|")[1]
        self.xx = rx
        fdata2 = data2.split("|")[0]
        ry = data2.split("|")[1]
        self.yy = ry
        self.x = pd.read_csv(fdata1)[rx]
        self.y = pd.read_csv(fdata2)[ry]
        if rx == "Date" or rx == "date" or rx == "DATE":
            try:
                self.x = pd.to_datetime(self.x, format=self.get_date_format(self.x[0].astype(str)))
            except:
                self.x = pd.to_datetime(self.x, format=self.get_date_format(self.x[0]))
        if ry == "Date" or ry == "date" or ry == "DATE":
            try:
                self.y = pd.to_datetime(self.y, format=self.get_date_format(self.y[0].astype(str)))
            except:
                self.y = pd.to_datetime(self.y, format=self.get_date_format(self.y[0]))
        self.df = pd.DataFrame({self.x.name: self.x, self.y.name: self.y})
        self.df = self.df.reset_index(drop=True)
        
    def day_interpolation(self):
        self.df = self.df.set_index(self.xx)
        self.df = self.df.resample('D')
        self.df = self.df.interpolate(method='linear')
        self.df = self.df.reset_index()
        return self.df
                        
            
        
    def main(self):
        if self.day_interpolation() is not None:
            self.day_interpolation().to_csv(self.xx + "_"+ self.yy + "_dayinter.csv", index=False)
        else:
            return None


# py "C:\Users\MrM\Desktop\New Thesis\main.py" 1 "C:\Users\MrM\Desktop\internship project\mydata\CSX.csv|DATE" "C:\Users\MrM\Desktop\internship project\mydata\CSX.csv|CSX1" 1
#dayinter(r"C:\Users\MrM\Desktop\internship project\mydata\CSX.csv|DATE", r"C:\Users\MrM\Desktop\internship project\mydata\CSX.csv|CSX1").main()
