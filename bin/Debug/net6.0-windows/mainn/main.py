import DBFtoCSV as db
import dayinter as di
import interpolator as ip
import harryplotter as hp
import pandas as pd
import sys


funcsele = sys.argv[1]


# py Main.py 1 C:\Users\MrM\Desktop\internship project\mydata\CSX.csv|DATE C:\Users\MrM\Desktop\internship project\mydata\CSX.csv|CSX1 1
# py "C:\Users\MrM\Desktop\New Thesis\main.py" 1 "C:\Users\MrM\Desktop\internship project\mydata\CSX.csv|DATE" "C:\Users\MrM\Desktop\internship project\mydata\CSX.csv|CSX1" 1
if funcsele == "1":
    x = sys.argv[2]
    y = sys.argv[3]
    plot = sys.argv[4]
    if plot == "1":
        hp.harryplotter(x, y, 1).main()
    elif plot == "2":
        hp.harryplotter(x, y, 2).main()
    elif plot == "3":
        hp.harryplotter(x, y, 3).main()
    elif plot == "4":
        hp.harryplotter(x, y, 4).main()
    elif plot == "5":
        hp.harryplotter(x, y, 5).main()
    elif plot == "6":
        hp.harryplotter(x, y, 6).main()
    elif plot == "7":
        hp.harryplotter(x, y, 7).main()
    elif plot == "8":
        hp.harryplotter(x, y, 8).main()
elif funcsele == "2":
    x = sys.argv[2]
    y = sys.argv[3]
    di.dayinter(x, y).main()
elif funcsele == "3":
    x = sys.argv[2]
    y = sys.argv[3]
    ip.interpolator(x, y).main()
'''if funcsele == "1":
    path = sys.argv[2]
    db.DBFtoCSV(path).main()
elif funcsele == "2":
    path = sys.argv[2]
    dfn = pd.read_csv(path)
    x = dfn[sys.argv[3]]
    y = dfn[sys.argv[4]]
    done = pd.DataFrame(di.dayinter(x, y).main())
    done.to_csv(r".\dayinterpolated.csv")
elif funcsele == "3":
    dfn1 = pd.read_csv(sys.argv[2])
    dfn2 = pd.read_csv(sys.argv[3])
    newdf = pd.DataFrame(ip.interpolator(dfn1, dfn2).main())
    newdf.to_csv(r".\interpolated.csv")
elif funcsele == "4":
'''