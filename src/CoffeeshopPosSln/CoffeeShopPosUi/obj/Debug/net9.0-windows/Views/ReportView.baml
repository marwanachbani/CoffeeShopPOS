   M S B A M L   `   `   `  ����J  FCoffeeShopPosUi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:CoffeeShopPosUi.ViewsCoffeeShopPosUi.Views  5   	   &     CoffeeShopPosUi.Views.ReportView   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:CoffeeShopPosUi.Views     5   	     � Title$  
ReportView��5   	   $	��450��6   $	��800��6)   .��5	      � .+�5
   
   �� .+�5      ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,10q�62   .+�5      (  "System.Windows.Controls.DatePicker     SelectedDate 6   �� *	StartDate+  5      $��10,0,0,0q�6>    6   �� *	EndDate+6>   �� 5      $��Generate Reports��6   $��10,0,0,0q�6_   ��64   �� *GenerateReportsCommand+6_   5      9� 5      $��Order Reports��6    9� 
IsExpanded$
 True��6.   .��5      &   System.Windows.Controls.DataGrid     AutoGenerateColumns$
 True��6@   ��6   �� *OrderReports+6@   5      9� 5      $��Session Reports��6   $
 True��60   .��5        $
 True��6B   ��6   �� *SessionReports+6B   5      9� 5      $��Payment Reports��6   $
 True��60   .��5        $
 True��6B   ��6   �� *PaymentReports+6B   5      5      5      5      