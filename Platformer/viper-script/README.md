# viper-script

Питоноподобный скриптовый язык, созданный, чтобы управлять сущностями в платформере.

## Пример кода:

Подсчёт количества простых чисел на промежутке от 1000 до 10000

```
// Переменные не надо объявлять заранее
begin = 1000
end =  10000

count = 0
// Отступы вместо скобок
while begin < end:
    begin += 1
    b = 2
    flag = True
    while b < bigin:
        if begin % b == 0:
            flag = False
        b += 1
    if flag:
        count += 1
print(count)
```
