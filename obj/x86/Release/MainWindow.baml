   M S B A M L   `   `   `  ���� C  ?ToDoList, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:ToDoListToDoList      ToDoList.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     )chartsclr-namespace:ToDoList     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     � Title$  
MainWindow��$	��645��$	��783�� � WindowStartupLocation$ CenterScreen=� � WindowState$ Normal=� J� OpacityMask
 �   � � Icon$+ %/ToDoList;component/Images/pencil.jpg�� � Left$
 Auto�� � 
ResizeMode$ CanMinimize=� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$
 Undo��  -   $
 Redo��	 J� InputBindings	 �� 
 �� Command$

 Undo��
 �� Key$ Z�� �� 	Modifiers$
 Ctrlf��� $

 Redo��$ Y��$
 Ctrlf�����  �� ImageSource$ Images\images (4).jpg�� j� Stretch$ UniformToFill=� j� TileMode$ FlipXY=� j� Viewport$+ %0,0,533.333333333333,106.666666666667� j� ViewportUnits$ Absolute=�.��� $	��699��$	��883�� � RowDefinitions �� $
��393* ��� $
��211* ��� $	��95* � � ColumnDefinitions �� $��2* ��� $
��782* ��� $	��99* �.+�&   System.Windows.Controls.DataGrid  -   $��	dataGrid1��
�������
�������$��3q�   CanUserDeleteRows . $��Cambria)�$��14(�$��Normal&�$
��Bold"�
���   �$��2��   HorizontalGridLinesBrush
 �   �   
IsReadOnly . $��20,304,33,53q�
 �   �   VerticalGridLinesBrush
 �   �$��1��   AreRowDetailsFrozen .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto    CanUserAddRows �� ���� *dateEventObj+  -   $��datagrid��$��20,304,33,52q� . 
 �   �$��3q�
����������	#0BF5F2F2
 �   �
 �   �$
��Bold"�$��Cambria)�$��14(�$��Normal&�
���   � . $��2��   GridLinesVisibility$	 All=�$ *  .     SelectionMode$ Extended=�$��1�� �� ���� *eventObject+�� -   $��label4��$��69��$��571,0,0,263q�$��Bottom=�$��1�����	#FFF2F1E6$��	Rawengulk)�$
��Bold"�$��14(�$��SemiExpanded&�$
��Left=�$	��177����Z� �� -   $��listBox1��$��82��$
��Left=�$��
18,0,0,164q� �� SelectionMode$ Single=�$��1�����	#FF051001
�������$��Normal"� J� Opacity$ 2Q�$	��117��$��Bottom=����� *dateEventObj+&   System.Windows.Controls.Calendar  -   $��	calendar1��$	��168��$
��Left=�$��
533,96,0,0q�$	��Top=�$	��255��$��1��   DisplayMode$ Month=�    SelectionMode$  
SingleDate=�!   IsTodayHighlighted! . 
���   �$��1q����	#FFFFF8F8$��2��o� -	   $��menu1��$��23��$
��Left=�$��
448,31,0,0q�$	��Top=�$	��300��" J� 	AllowDrop" . $��1�����	#FFFCFCFC.@�m� -
   $
��View��$��_View��" . $	��100��$y�Visible=�.@�m� -   $��History��$y�Visible=�$��History��m� -   $��Performance��$y�Visible=�$��Performance��m� -   $
��Help��$��  _Help��" . $	��100��.@�m� -   $��About��$��About What'sNext��m� -   $��	UserGuide��$��
User Guide��m� -   $��Commands��$��Commands��m� -   $
��Quit��$��_Quit��" . $	��100���� -   $��image2��$��66��$
��Left=�$��	19,14,0,0q�$
��Fill=�$	��Top=�$��62��$��1��$+��%/ToDoList;component/Images/pencil.jpg��`� -   $��toolBarTray1��$��40��$
��Left=�$��
306,92,0,0q�$	��Top=�$	��280��$��2��# `� 
Background# �	#00400000$��2���� -   $��label3��$��Enter Command��$��34��$
��Left=�$��	19,86,0,0q�$	��Top=�$	��174��$��1��$��Chiller)�$��26(�$
��Bold"����	#FFF8F8F8��Z� �� -   $��textBox2��$	��156��$
��Left=�$��359,148,0,0q�$	��Top=�$	��772��$ �� TextWrapping$
$ Wrap=�$��1�� �	#75000000���	#E6F8ECEC$��2��$��2��.������ $ Images\woodbackground.jpg��$
 Fill=�$ FlipXY=�$ 0,0,1024,1024�$ Absolute=�.���� -   $��textBox1��$��24��$
��Left=�$��
18,126,0,0q�$	��Top=�$	��531��$��""��% �� AutoWordSelection% . {�. $��1��
���   ����	#FF0D0000 �	#FF0A0801$��Cambria)�$��14(�
�������& � DataContext& �� .��' � ToolTip' ]� $
��Auto��$
��Auto��.+��� <:To add an event, enter event description within the quotes.���� $��What'sNext?��$��Cambria)�$��28(�$��Normal$�$
��Bold"�$��1��$��40��$��Right=�$��
0,14,472,0q�$	��Top=�$	��223�����	#FFFCFBF2$��SemiExpanded&��� $��Plan your life ahead..��$��Book Antiqua)�$��13(�$��1��$��30��$��Right=�$��
0,50,494,0q�$	��192�����	#FFFCFAEF$	��Top=�$��SemiExpanded&��� $��Display Box��$��Chiller)�$��22(�$
��Bold"�$��1��$��43��$
��Left=�$��
20,266,0,0q�$	��Top=�$��96�����	#000A0700���	#FFF5F4F2�� -   $��ToastMsg��$��1��$��26��$
��Left=�$��
20,234,0,0q�$	��Top=�$	��529��
����������	#00000000$��Segoe UI)�$��14(�.������ .���� -   $��label1��$D��>Copyright © by Pirates Of Silicon Valley.All rights reserved.��$��1��$��1��$��2��$��28��$
��Left=�$��223,188,0,0q�$	��Top=�$	��324��
�������$��10(�$��Arial)�`� -   $��toolBarTray2��$��1��$��32��$
��Left=�$��583,266,0,0q�$	��Top=�$	��166��$y�Visible=� �	#00000000�� -   $��image1��$��1��$��24��$��653,274,102,0q�$
��Fill=�$	��Top=�$.��(/ToDoList;component/Images/Edit-icon.png�� �	#FF100E0E' ]� $
��Auto��$
��Auto��.+��� Edit�� -   $��image6��$��1��$��24��$
��Left=�$��686,274,0,0q�$
��Fill=�$	��Top=�$��27��$2��,/ToDoList;component/Images/PowerDel.icon.gif��' ]� $
��Auto��$
��Auto��.+��� Delete�� -   $��image7��$��1��$��24��$
��Left=�$��
721,0,0,94q�$
��Fill=�$��Bottom=�$��27��$/��)/ToDoList;component/Images/1505202069.png��' ]� $
��Auto��$
��Auto��.+��� Done�� -   $��image8��$��1��$��1��$��39��$
��Left=�$��708,165,0,0q�$
��Fill=�$	��Top=�$��42��$;��5/ToDoList;component/Images/Right_Arrow_Icon_Black.png��' ]� $
��Auto��$
��Auto��.+��� 	Flip View�� -   $��performanceLabel��$�� ��$��1��$	��361��$
��Left=�$��
96,0,0,149q�$��Bottom=�$	��570��$y�Hidden=�$ 1Q�
�������$��2���� -   $��performanceButton��$��Close��$��1��$��23��$
��Left=�$��
598,95,0,0q�$	��Top=�$��67��$y�Hidden=�   ToDoList.PerformanceChart  -    (  Name$( performanceChart1��$��1��$	��350��$
��Left=�$��
130,74,0,0q�$	��Top=�$	��550��$y�Hidden=�$��2���� -!   $
��test��$
��Test��$��1��$��1��$��23��$
��Left=�$��615,173,0,0q�$	��Top=�$��75��$y�Visible=����	#04000000���	#FF0E0101���	#FF0A0000
 �   �