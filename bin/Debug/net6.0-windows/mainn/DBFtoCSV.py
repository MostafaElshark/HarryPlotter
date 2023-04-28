import pandas as pd
from simpledbf import Dbf5
import os
from pathlib import Path
import re

class DBFtoCSV:
    def __init__(self, path):
        self.path = path
        self.files = []
        self.df = None
        self.name = None
        self.date = None

    def read_files(self):
        for file in os.listdir(self.path):
            if file.endswith(".dbf"):
                self.files.append(Path(self.path).joinpath(file))
    
    def get_df_name(self, df):
        self.name = Path(df).stem
        return self.name
    
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
        elif re.match(r"D_\d{4}\d{2}\d{2}$", date):
            return "D_%Y%m%d"
        else:
            return None
        
    def VtoH(self, df):
        self.tota = self.get_df_name(df)
        self.df = Dbf5(df).to_dataframe()
        self.df = self.df.stack().reset_index()
        self.df.columns = ['index','Date', self.tota]
        self.df = self.df.drop('index', axis=1)
        self.df = self.df.iloc[1:]
        self.df = self.df.reset_index(drop=True)
        return self.df
    
    def get_date(self, df):
        for i in range(len(df)):
            if self.get_date_format(df['Date'][i]) is not None:
                df['Date'][i] = pd.to_datetime(df['Date'][i], format=self.get_date_format(df['Date'][i])).date()
            else:
                df = df.drop(i, axis=0)
        return df
    
    def main(self):
        self.read_files()
        for i in self.files:
            self.df = self.VtoH(i)
            self.df = self.get_date(self.df)
            self.df.to_csv(Path(self.path).joinpath(self.get_df_name(i)+'.csv'), index=False)


#DBFtoCSV(r"C:\Users\MrM\Desktop\New Intership\NewData\clear_data_gonyu\ori\clear_data_gonyu").main()