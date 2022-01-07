import requests
import json

word = "particular"
r = requests.get('https://ac-dict.naver.com/enko/ac?st=11&r_lt=11&q='+word)
if(r.status_code != 200):
    print("Error, Status code isn't 200, {}".format(r.status_code))
else:
    res = r.json()
    #[ [ word, space, [means]] [ syn word , space, [means]] ]
    word = res['query'][0]
    meanings = [m.strip() for m in res['items'][0][0][2][0].split(',')]
    
    print(word)
    for m in meanings:
        print(m)
    
    

