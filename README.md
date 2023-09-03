# Invert
## Русский
### Подготовь свой конфиг к CS2

В общем написал небольшую штуку, которая будет конвертировать конфиг CS:GO в конфиг CS2.
Все что нужно сделать - так это загрузить имеющийся конфиг и путь на который нужно сохранять новый конфиг.
Далее необходимо перейти во вкладку Работа (Work) и нажать на Фикс (Fix). В этот момент будет выполнен анализ конфига и в логе слева будут выводится изменения.
В таблице с конфигурацией можно выключать или включать ту или иную строку конфигурации.
После этого необходимо нажать Сохранить (Save) и новый конфиг с прооизвольным именем сохранится в той папке, которую вы указали на выход.

Преимущства:
* Чистка конфига от устаревших/неработаюших команд
* Удаление устаревших/неработаюших аргументов команд
* Удаление дубликатов
* Удаление/отключение alias и ссылок на них
* Поддержка 2-ух языков (английский и русский) p.s. - не знаю зачем, мне просто было весело писать

Известные проблемы:
* UI (может починю, а может нет. И так сойдет)
* Unbindall похоже ломает движение мышкой (пока что он добавлен в список очистки, т.е. продукт будет автоматически предлагать убрать эту строку из конфига)

Текущая сборка (1.0.0-alpha.2) является **тестовой** и могут наблюдаться различные баги и проблемы.
Если вы нашли проблему, то просьба сообщить о ней через "Issues" гитхаба.

Скачивая и запуская этот продукт вы подтвержаете, что все дальнейшие действия с продуктом Intern вы делаете на свой страх и риск.
Я не несу ответственности за любую случившую неполадку с вашими данными и вашим компьютером.

Продукт не заставляет старые бинды работать. К примеру cl_righthand есть как команда, но она не работает и работать не будет.

Данная сборка тестировалась на конфигах для CS2 версии *2000111/13928*

> Лучшая благодарность - звездочка в репозиторий

## English
### Prepare your config for CS2

In general, I wrote a small thing that will convert the CS:GO config to the CS2 config.
All you need to do is load the existing config and the path where you want to save the new config.
Next, you need to go to the tab Work (Work) and click on Fix (Fix). At this point, the config will be analyzed and changes will be displayed in the log on the left.
In the configuration table, you can disable or enable one or another configuration line.
After that, you need to click Save (Save) and the new config with an arbitrary name will be saved in the folder that you specified to exit.

Advantages:
* Cleaning the config from obsolete / broken commands
* Remove obsolete/broken command arguments
* Remove duplicates
* Removing/disabling alias and links to them
* Support for 2 languages (English and Russian) p.s. I don't know why, I just had fun writing

Known issues:
* UI (maybe fix it, maybe not. And so it will do)
* Unbindall seems to break mouse movement (so far it has been added to the cleanup list, i.e. the product will automatically offer to remove this line from the config)

The current build (1.0.0-alpha.2) is a **test** build and may experience various bugs and issues.
If you find a problem, please report it through the "Issues" github.

By downloading and running this product, you confirm that you do all further actions with the Intern product at your own peril and risk.
I am not responsible for any failure that occurs with your data and your computer.

The product does not force old binds to work. For example, cl_righthand is available as a command, but it does not work and will not work.

This assembly was tested on configs for CS2 version *2000111/13928*

> Best thanks - star to repository