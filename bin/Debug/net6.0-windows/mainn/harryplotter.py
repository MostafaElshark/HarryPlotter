import pandas as pd
import matplotlib.pyplot as plt
from statsmodels.graphics import tsaplots
import seaborn as sns
import numpy as np; np.random.seed(42)
from pandas.plotting import autocorrelation_plot
from scipy.stats import pearsonr, spearmanr
from scipy import stats
import re
import os

class harryplotter():
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
    
    def __init__(self, data1, data2, plot):
        fdata1 = data1.split("|")[0]
        rx = data1.split("|")[1]
        fdata2 = data2.split("|")[0]
        ry = data2.split("|")[1]
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
        self.plot = plot
        self.path = os.getcwd()
    
    def scatterplot(self):
        sns.set()
        plt.scatter(self.x, self.y)
        plt.xlabel(self.x.name)
        plt.ylabel(self.y.name)
        plt.title("X= "+self.x.name + " And Y= " + self.y.name)
        fig = plt.gcf() # get current figure
        fig.set_size_inches(32, 18) # set figure's size manually to your full screen (32x18)
        plt.draw() # draw the plot
        fig.savefig(self.path + "\/scatterplot X=" + self.x.name + " Y=" + self.y.name + ".png", dpi=200, bbox_inches='tight') # save the plot to file
        plt.clf() # clear the plot area
    
    def lineplot(self):
        sns.set()
        plt.plot(self.x, self.y)
        plt.xlabel(self.x.name)
        plt.ylabel(self.y.name)
        plt.title("X= "+self.x.name + " And Y= " + self.y.name)
        fig = plt.gcf()
        fig.set_size_inches(32, 18) # set figure's size manually to your full screen (32x18)
        plt.draw() # draw the plot
        fig.savefig(self.path + "\/lineplot X=" + self.x.name + " Y=" + self.y.name + ".png", dpi=200, bbox_inches='tight') # save the plot to file
        plt.clf() # clear the plot area
    
    def histogram(self):
        sns.set()
        plt.hist(self.x)
        plt.xlabel(self.x.name)
        plt.ylabel("Frequency")
        plt.title("Histogram of " + self.x.name)
        fig = plt.gcf()
        fig.set_size_inches(32, 18)
        plt.draw()
        fig.savefig(self.path + "\/histogram X=" + self.x.name + ".png", dpi=200, bbox_inches='tight') # save the plot to file
        plt.clf() # clear the plot area
    
    def boxplot(self):
        sns.set()
        plt.boxplot(self.x)
        plt.xlabel(self.x.name)
        plt.ylabel("Frequency")
        plt.title("Boxplot of " + self.x.name)
        fig = plt.gcf()
        fig.set_size_inches(32, 18)
        plt.draw()
        fig.savefig(self.path + "\/boxplot X=" + self.x.name + ".png", dpi=200, bbox_inches='tight')
        
    def autocorrelation(self):
        sns.set()
        autocorrelation_plot(self.x)
        plt.xlabel(self.x.name)
        plt.ylabel("Frequency")
        plt.title("Autocorrelation of " + self.x.name)
        fig = plt.gcf()
        fig.set_size_inches(32, 18)
        plt.draw()
        fig.savefig(self.path + "\/autocorrelation X=" + self.x.name+ ".png", dpi=200, bbox_inches='tight')
        plt.clf()
    
    def acf(self):
        sns.set()
        tsaplots.plot_acf(self.x)
        plt.xlabel(self.x.name)
        plt.ylabel("Frequency")
        plt.title("Autocorrelation of " + self.x.name)
        fig = plt.gcf()
        fig.set_size_inches(32, 18)
        plt.draw()
        fig.savefig(self.path + "\/acf X=" + self.x.name+ ".png", dpi=200, bbox_inches='tight')
        plt.clf()
    
    def pacf(self):
        sns.set()
        tsaplots.plot_pacf(self.x)
        plt.xlabel(self.x.name)
        plt.ylabel("Frequency")
        plt.title("Autocorrelation of " + self.x.name)
        fig = plt.gcf()
        fig.set_size_inches(32, 18)
        plt.draw()
        fig.savefig(self.path + "\/pacf X=" + self.x.name + ".png", dpi=200, bbox_inches='tight')
        plt.clf()
    
        
    def correlation(self):
        slope, intercept, r_value, p_value, std_err = stats.linregress(self.x,self.y)
        with open(self.path + '\Correlation.txt', 'a') as f: # Open the file
            f.write("Correlation of X="+self.x.name+" and Y="+self.y.name+"\n") # Write the title
            f.write(str(pearsonr(self.x,self.y))+"\n") # Write the spearmanr
            f.write(str(spearmanr(self.x,self.y))+"\n")
            f.write("Correlation: "+ str(self.x.corr(self.y))+"\n") # Write the correlation
            f.write("Covariance: "+ str(self.x.cov(self.y))+"\n\n") # Write the covariance
            f.write("slope: %f    intercept: %f" % (slope, intercept)+"\n")
            f.write("r-squared: %f" % r_value**2+"\n")
            f.write("p_value: %f" % p_value+"\n")
            f.write("std_err: %f" % std_err+"\n")
            f.write("")
            f.write("")
            f.close()


    def main(self):
        if self.plot == 1:
            self.scatterplot() # x and y are the columns
        elif self.plot == 2:
            self.lineplot() # x and y are the columns
        elif self.plot == 3:
            self.histogram() # x is the column
        elif self.plot == 4:
            self.boxplot() # x is the column
        elif self.plot == 5:
            self.autocorrelation() # x is the column
        elif self.plot == 6:
            self.acf() # x is the column
        elif self.plot == 7:
            self.pacf() # x is the column
        elif self.plot == 8:
            self.correlation() # x and y are the columns
        else:
            print("Please enter a valid plot type")