   M S B A M L   `   `   `  ���� K  GSystemMonitoring, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:SystemMonitoring.AdminPagesSystemMonitoring.AdminPages  *    $SystemMonitoring.AdminPages.Cultures   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ;local)clr-namespace:SystemMonitoring.AdminPages     $
��auto��  N� Title$  Cultures��.���� .���  � RowDefinitions �� $��35 ��� $	��350 ��� $��35 ��� $	��350 ��� $��35 ��� $	��350 �.+���  �� Orientation$ 
Horizontal=�.+��� -   $��SelectCultureSeed��$	��250��$��25��$��0,0,10,0q��� -   $��SelectCultureStatus��$	��250��$��25��$��0,0,10,0q�{�.  �� -    ��Name$ FilterCulture��$��Фильтровать��&   System.Windows.Controls.DataGrid  -     Name$ 
DGCultures��$��1��   AutoGenerateColumns .     
IsReadOnly .    Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Культура��	   Width5  /System.Windows.Controls.DataGridLengthConverter$		 200 1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 �� *Name+  $ Статус посева��$		 100 
 �� *Status+  $ Период��$	 60 
 �� *Period+  $ Температура��$	 90 
 �� *Temperature+  $ Влажность��$	 90 
 �� *
Humidity+  $ Фосфор��$	 80 
 �� *
Phosphor+  $ Кальций��$	 80 
 �� *	Calcium+  $ 
Калий��$	 80 
 �� *	Potassium+  $ Азот��$	 70 
 �� *
Nitrogen+  $ Магний��$	 80 
 �� *	Magnesium+  $ Кислотность��$		 100 
 �� *Ph+4  .System.Windows.Controls.DataGridTemplateColumn  $	 25    CellTemplate �� .R��� -      BtnEdit#��[  $��25��  -   $ DGFertilizer��$��3�� .   .    $ Наименование��$		 200 
 �� *Name+  $ Фосфор��$	 80 
 �� *PhosphorusOxide+  $ Кальций��$	 80 
 �� *CalciumOxide+  $ 
Калий��$	 80 
 �� *PotassiumOxide+  $ Азот��$	 70 
 �� *Asot+  $ Магний��$	 80 
 �� *MagnesiumOxide+  $ Кислотность��$		 100 
 �� *NitrateNitrogen+  $	 25  �� .R��� -   #��[  $��25��