   M S B A M L   `   `   `  ����   SystemMonitoring&     SystemMonitoring.FieldMonitoring   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    >d2http://schemas.microsoft.com/expression/blend/2008    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     $
��1600��  N� Title$  FieldMonitoring��.���  � RowDefinitions �� $��30 ��� $��30 ��� $��30 ��� $��30 ��� $��30 ��� $��30 ��� $��30 ��� $��20 ��� $
��350* �.+��� $��0��$	��300��$
��Left=�$��Center=� �� Orientation$ 
Horizontal=�$��60,0,0,0q�.+��� $��Район:�� 	  Value#��[  $	��5,0q��� -    ��Name$ District��#��[  $	��5,0q����� *Field.District+�� $��1��$	��300��$
��Left=�$��Center=�$ 
Horizontal=�$��60,0,0,0q�.+��� $��	Поле:��#��[  $	��5,0q��� -   $ Number��#��[  $	��5,0q����� *Field.Number+�� $��2��$	��300��$
��Left=�$��Center=�$ 
Horizontal=�$��10,0,0,0q�.+��� $��Культура:��#��[  $	��5,0q��� -    ��Name$ 	CultureCB��$	��210��$	��5,0q��� $��3��$	��350��$
��Left=�$��Center=�$ 
Horizontal=�$��10,0,0,0q�.+��� $��Статус:��#��[  $	��5,0q��� -   $ Status��#��[  $	��5,0q��� $��4��$	��300��$
��Left=�$��Center=�$ 
Horizontal=�$��10,0,0,0q�.+��� $��Дата посева:��#��[  $	��5,0q��� -   $
 Date��$	��200��#��[  ���� *Date+�� $��5��$	��300��$
��Left=�$��Center=�$ 
Horizontal=�$��10,0,0,0q�.+��� $��Тип почвы:��#��[  $	��5,0q��� -   $
 Soil��$	��198��$	��5,0q��� $��6��$ 
Horizontal=�$
��10,0q�$��Center=�$
��Left=�.+��� -    ��Name$ Refresh��  Refresh#��[ $��0,0,10,0q� � ToolTip _� $?��9Обновить информацию о датчиках���� -   $	 Add��  Add#��[ $��0,0,10,0q� _� $.��(Добавить новый датчик���� -	   $
 Save��  BtnSave#��[ $��0,0,10,0q� _� $'��!Сохранить датчики���� -
   $ SaveToExcel��  BtnExcel#��[ $��0,0,10,0q� _� $C��=Сохранить информацию по датчикам���� -   $ SaveToExcelSensors��#��[ $��0,0,10,0q� _� $A��;Сохранить информацию по датчику���� $ 
Horizontal=�$��7��$
��10,0q�$��Center=�.+��� $��ID��$��50�� �� TextAlignment$ Center=�$��0,0,1,0q��� $��Влажность, %��$��75��$ Center=�$��0,0,1,0q��� $!��Температура, °C��$��95��$ Center=�$��0,0,1,0q��� $��Кислотность��$��80��$ Center=�$��0,0,1,0q��� $��Фосфор, мг/кг��$��85��$ Center=�$��0,0,1,0q��� $��Кальций, мг/кг��$��85��$ Center=�$��0,0,1,0q��� $��Магний, мг/кг��$��85��$ Center=�$��0,0,1,0q��� $��Калий, мг/кг��$��70��$ Center=�$��0,0,1,0q��� $��Азот, мг/кг��$��60��$ Center=�$��0,0,1,0q��� $ 
Horizontal=�$��Right=�$��7��$
��10,0q�.+��� $-��'Датчиков обнаружено: ���� -    ��Name$ Count��$��0���� $��0q�	 �� CornerRadius$	 10��$��8�����#DFDFDF$	��Top=�$	��300��$
��10,3q�._��� $��Hidden=�$	��0,5q�.���� -   
 ��Name$
 Sensors��