   M S B A M L   `   `   `  ����F  BCard Reader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Card_ReaderCard_Reader  5   	       Card_Reader.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Card_Reader     5   	     � Title$  Card Reader��5   	   $	��520��6   $	��680��6*   ���	#FF111111
�������.��5	      � .+�5
      �� -   5         ��Name$ OpenFile��5
      $��	Open File��6   $
��Left=�61   $��	38,20,0,0q�6L   $	��Top=�6_   $��75��6w   ���	#FFDDDDDD���	#FF2727276�   �� 5      -   5        $ SaveFile��5      $��	Save File��6   $
��Left=�61   $��	38,57,0,0q�6L   $	��Top=�6_   $��75��6w   ���	#FFDDDDDD���	#FF2727276�   �� 5      -   5        $ SaveCard��5      $��	Save Card��6   $
��Left=�61   $��	38,95,0,0q�6L   $	��Top=�6_   $��75��6w   ���	#FFDDDDDD���	#FF2727276�   �� 5      -   5        $ AddDeck��5      $��Add Deck��6   $
��Left=�6/   $��
38,135,0,0q�6J   $	��Top=�6^   $��75��6v   ���	#FFDDDDDD���	#FF2727276�   �� 5      -   5        $ AddCard��5      $��Add Card��6   $
��Left=�6/   $��
38,175,0,0q�6J   $	��Top=�6^   $��75��6v   ���	#FFDDDDDD���	#FF2727276�   �� 5      -   5         ��Name$ CardListLabel��5   
   $��	Card List��6!   $
��Left=�65   $��
38,211,0,0q�6P   $	��Top=�6d   ���	#FFDDDDDD$��24��6�   �� 5      -   5         ��Name$ CardList��5      $
��Left=�6   $	��215��69   $��
38,240,0,0q�6F   $	��Top=�6Z   $	��280��6r   ���	#FF272727���	#FFDDDDDD6�   �� 5      -   5        $ CurrentDeckLabel��5   
   $��Current Deck:��6$   $
��Left=�6<   $��
143,17,0,0q�6W   $	��Top=�6k   ���	#FFCBCBCB6�   �� 5      -	   5         ��Name$ CurrentDeck��5      $
��Left=�6#   $��
222,22,0,0q�6>   $
��Wrap=�6R   $��No Deck��6f   $	��Top=�6u   
������� J� OpacityMask
 �   �$
��Bold"�6�   �� 5      -
   5        $ NewDeckLabel��5   
   $��New Deck Name:��6    $
��Left=�69   $��
123,54,0,0q�6T   $	��Top=�6h   ���	#FFCBCBCB6�   �� 5      -   5         ��Name$ 
NewDeckBox��5      $
��Left=�6    $��23��6;   $��
222,57,0,0q�6G    �� TextWrapping$
 Wrap=�6[   $	��Top=�6o   $��96��6�   ���	#FFDDDDDD���	#FF2727276�   �� 5      -   5        $ CurrentCardLabel��5   
   $��Current Card:��6$   $
��Left=�6<   $��
143,95,0,0q�6W   $	��Top=�6k   ���	#FFCBCBCB6�   �� 5      -   5        $ CurrentCard��5      $
��Left=�6#   $��222,100,0,0q�6>   $
��Wrap=�6S   $��No Card��6g   $	��Top=�6v   
�������
 �   �$
��Bold"�6�   �� 5      -   5        $ NewCardLabel��5   
   $��New Card Name:��6    $
��Left=�69   $��123,132,0,0q�6T   $	��Top=�6i   ���	#FFCBCBCB6�   �� 5      -   5        $ 
NewCardBox��5      $
��Left=�6    $��23��6;   $��222,135,0,0q�6G   $
 Wrap=�6\   $	��Top=�6p   $��96��6�   ���	#FFDDDDDD���	#FF2727276�   �� 5      -   5        $ CurrentDescriptionLabel��5   
   $��Current Description��6+   $
��Left=�6I   $��364,158,0,0q�6d   $	��Top=�6y   ���	#FFDDDDDD6�   �� 5      -   5        $ CurrentDescriptionBox��5      $
��Left=�6+   $	��115��6F   $��364,189,0,0q�6S   $
 Wrap=�6h   $��Description��6|   $	��Top=�6�   $	��274��6�   ���	#FF272727���	#FFDDDDDD �� 
IsReadOnly . 6�   �� 5      -   5        $ NewDescriptionLabel��5   
   $��New Description��6'   $
��Left=�6A   $��364,309,0,0q�6\   $	��Top=�6q   ���	#FFDDDDDD6�   �� 5      -   5        $ NewDescriptionBox��5      $
��Left=�6'   $	��115��6B   $��364,340,0,0q�6O   $
 Wrap=�6d   $	��Top=�6x   $	��274��6�   ���	#FF272727���	#FFDDDDDD6�   	� 5       -   5        	 	�Name$	 RB_NoTarget��5       $��	No Target��6%   $
��Left=�69   $��
364,20,0,0q�6T   $	��Top=�6h   ���	#FFDDDDDD���	#FF272727
 d� 	IsChecked$

 TrueY�6�   	� 5!      -   5        $	 RB_SelfTarget��5!      $��Self Target��6'   $
��Left=�6=   $��
364,62,0,0q�6X   $	��Top=�6l   ���	#FFDDDDDD���	#FF2727276�   	� 5"      -   5        $	 RB_TargetOthers��5"      $��Target Others��6)   $
��Left=�6A   $��364,106,0,0q�6\   $	��Top=�6q   ���	#FFDDDDDD���	#FF2727276�   �� 5#      -   5        $ OldAttributeLabel��5#   
   $��Old Attribute:��6%   $
��Left=�6>   $��
482,65,0,0q�6Y   $	��Top=�6m   ���	#FFDDDDDD6�   �� 5$      -   5        $ OldAttributeBox��5$      $
��Left=�6%   $��22��6@   $��
590,69,0,0q�6L   $
 Wrap=�6`   $	��Top=�6t   $��48��6�   ���	#FFDDDDDD���	#FF272727 . 6�   �� 5%      -   5        $ AttributeChangeLabel��5%   
   $��Attribute Change:��6(   $
��Left=�6D   $��482,102,0,0q�6_   $	��Top=�6t   ���	#FFDDDDDD6�   �� 5&      -   5        $ AttributeChangeBox��5&      $
��Left=�6(   $��22��6C   $��590,106,0,0q�6O   $
 Wrap=�6d   $	��Top=�6x   $��48��6�   ���	#FFDDDDDD���	#FF2727276�   o� 5(      -   5         o�Name$ AttributeMenu��5(   	   $
��Left=�6    $��40��6;   $��
482,20,0,0q�6G   $	��Top=�6[   $	��156��6s   ���	#FF272727���	#FFABADB3���	#FFDDDDDD$��1q�6�   .@�5)      m� -   5        $��No Attribute Change��5)      ���	#FFDDDDDD6B   m� 5*      $��Change Attribute��6   ���	#FFDDDDDD.@�5+      m� -   5        $��Money��5+      ���	#FF272727���	#FFDDDDDD���	#FFABADB36d   m� 5,      -   5        $��Users��5,      ���	#FF272727���	#FFDDDDDD���	#FFABADB36d   m� 5-      -   5        $��Memes��5-      ���	#FF272727���	#FFDDDDDD���	#FFABADB36d   m� 5.      -    5        $��	Bandwidth��5.      ���	#FF272727���	#FFDDDDDD���	#FFABADB36h   5/      50      51      52      