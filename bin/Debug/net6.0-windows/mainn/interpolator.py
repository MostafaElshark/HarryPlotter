import pandas as pd
import re
from pathlib import Path

class interpolator():
    # This class is used to interpolate the missing values in the time series data.
    # The data is interpolated using the linear interpolation method.
    # The class takes two arguments: dataframe1 and dataframe2.
    # The dataframes should have two columns: date and values.
    # The date column should be the first column and the values column should be the second column.
    
    def get_date_format(self, date):
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
        self.fdata1 = data1.split("|")[0]
        self.fdata2 = data2.split("|")[0]
        #ry = data2.split("|")[1]
        self.df1 = pd.read_csv(self.fdata1)
        self.df2 = pd.read_csv(self.fdata2)
        #y1 = data1.split("|")[1]
        #y2 = data2.split("|")[1]
        self.daten1 = self.df1.columns[0]
        self.daten2 = self.df2.columns[0]
        #self.namer1 = self.df1[y1]
        #self.namer2 = self.df2[y2]
        try:
            self.df1[self.daten1] = pd.to_datetime(self.df1[self.daten1], format=self.get_date_format(self.df1[self.daten1][1].astype(str)))
        except:
            self.df1[self.daten1] = pd.to_datetime(self.df1[self.daten1], format=self.get_date_format(self.df1[self.daten1][1]))
        try:
            self.df2[self.daten2] = pd.to_datetime(self.df2[self.daten2], format=self.get_date_format(self.df2[self.daten2][1].astype(str)))
        except:
            self.df2[self.daten2] = pd.to_datetime(self.df2[self.daten2], format=self.get_date_format(self.df2[self.daten2][1]))

    def removeNan(self, df):
        h = df.columns[1]
        for i in range(len(df)):
            if df[h][i] == df[h][i]:
                return df[i:]
    
    def removenanfromlast(self, df):
        h = df.columns[1]
        for i in range(len(df))[::-1]:
            if df[h][i] == df[h][i]:
                return df[:i+1]
            
    def get_df_name(self): # get the name of the file
        self.name1 = Path(self.fdata1).stem
        self.name2 = Path(self.fdata2).stem
    

    def interpolatetwo(self):
        self.df1 = self.df1.set_index(self.daten1)
        self.df2 = self.df2.set_index(self.daten2)
        newdf = self.df1.join(self.df2, how='outer')
        newdf = self.removeNan(newdf)
        newdf = self.removenanfromlast(newdf)
        newdf = newdf.interpolate(method='linear')
        newdf = newdf.reset_index()
        return newdf
    
    def main(self):
        self.get_df_name()
        if self.interpolatetwo is not None:
            self.interpolatetwo().to_csv(self.name1 + "_" + self.name2 + "_interpolated"+".csv", index=False)
