# coding: utf-8

import sys
import re

def recdict(ld, i):
    if ld[i] == '}':
        return (None,i+1)
    elif ld[i+1] == '=':
        dd = {}
        while ld[i] != '}':
            if ld[i] == '{' and ld[i+1] == '}':
                i = i + 2
            elif ld[i] in dd and type(dd[ld[i]]) is tuple:
                if ld[i+2] != '{':
                    dd[ld[i]] = dd[ld[i]] + (ld[i+2],)
                    i = i + 3
                else:
                    tmp, it = recdict(ld, i+3)
                    dd[ld[i]] = dd[ld[i]] + (tmp,)
                    i = it
            elif ld[i] in dd:
                if ld[i+2] != '{':
                    dd[ld[i]] = (dd[ld[i]], ld[i+2])
                    i = i + 3
                else:
                    tmp, it = recdict(ld, i+3)
                    dd[ld[i]] = (dd[ld[i]], tmp)
                    i = it
            else:
                if ld[i+2] != '{':
                    dd[ld[i]] = ld[i+2]
                    i = i + 3
                else:
                    dd[ld[i]], it = recdict(ld, i+3)
                    i = it
        return (dd,i+1)
    else:
        ll = []
        while ld[i] != '}':
            if ld[i] != '{':
                ll = ll + ld[i].split()
                i = i + 1
            else:
                el, i = recdict(ld,i+1)
                ll = ll + [el]
        return (ll,i+1)

def token(raw):
    d = []
    s = False
    t = False
    b = 0
    raw = raw + ' '
    for i in range(len(raw)):
        if s and raw[i] == '"' and raw[i-1] != '\\':
            d.append(raw[b:i])
            s = False
        elif s:
            pass
        elif raw[i] == '"':
            if t:
                d.append(raw[b:i])
                t = False
            s = True
            b = i + 1
        elif raw[i] in ('{','}','='):
            if t:
                d.append(raw[b:i])
                t = False
                if raw[i] != '=':
                    d.append('=')
            d.append(raw[i])
        elif t and raw[i] in (' ','\t','\n','\r'):
            d.append(raw[b:i])
            t = False
        elif not t and raw[i] not in (' ','\t','\n','\r'):
            t = True
            b = i
    return d

def parsefile(filename):
    t = token(re.sub('#.*','',open(filename, 'r').read())) + ['}']
    if t[0][:3] == '\xef\xbb\xbf':
        t[0] = t[0][3:]
    dat, tp = recdict(t, 1 if t[0][-3:] == 'txt' else 0)
    if tp != len(t):
        raise Exception('Not all tokens were parsed: {}/{}'.format(tp, len(t)))
    return dat


dat = parsefile('00_landed_titles.txt')

out = ''
max_prov = 0
num_county = 0

for e in dat:
    if e[:2] == 'e_':
        for k in dat[e]:
            if k[:2] == 'k_':
                for d in dat[e][k]:
                    if d[:2] == 'd_':
                        for c in dat[e][k][d]:
                            if c[:2] == 'c_':
                                num_county = num_county + 1
                                out = out + '\n' + c + ' '
                                out = out + str(len([b for b in dat[e][k][d][c] if b[:2] == 'b_'])) + ' '
                                for b in dat[e][k][d][c]:
                                    if b[:2] == 'b_':
                                        if type(dat[e][k][d][c][b]['province']) is type((0,)):
                                            dat[e][k][d][c][b]['province'] = dat[e][k][d][c][b]['province'][0]
                                        out = out + dat[e][k][d][c][b]['province'] + ' '
                                        max_prov = max(max_prov, int(dat[e][k][d][c][b]['province']))

out = str(num_county) + ' ' + str(max_prov) + out
open('merge_baronies.tmp', 'w').write(out)

open('definition.tmp', 'w').write(re.sub('#.*','',open('definition.csv', 'r').read()))

