   M S B A M L   `   `   `  ����F  BMusicPlayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:MusicPlayerMusicPlayer  5   	       MusicPlayer.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:MusicPlayer     5   	     � Title$  
MainWindow��5   	   $	��367��6   $	��536��6)   .��5	      � $��0,0,2,-5q�6   .+�5
   
   �� -   5         ��Name$ listView��5
      $��
10,63,10,0q�6%   $	��232��69   $	��Top=�6F    �� View 5      �� 5      .P�5      �� $��Title��6%    �� DisplayMemberBinding 64   �� *Title+�� 5      $��Artist��6%    65   �� *Artist+5      5      �� 5   
   -   5         ��Name$ previousButton��5      $��Previous��6)   $
��Left=�6<   $��
10,300,0,0q�6W   $	��Top=�6k   $��75��6�   6�   �� 5   
   -   5        $ 
playButton��5      $
��Play��6%   $
��Left=�64   $��
90,300,0,0q�6O   $	��Top=�6c   $��75��6{   6�   �� 5   
   -   5        $ 
stopButton��5      $
��Stop��6%   $
��Left=�64   $��170,300,0,0q�6O   $	��Top=�6d   $��75��6|   6�   �� 5   
   -   5        $ 
nextButton��5      $
��Next��6%   $
��Left=�64   $��250,300,0,0q�6O   $	��Top=�6d   $��75��6|   6�   �� 5   
   -   5         ��Name$ 	volumeBar��5      $��414,300,0,0q�6$   $	��Top=�69   $��20��6Q    � Maximum$	 100Q�6]    � SmallChange$ 1Q�6�    �� IsMoveToPointEnabled . $
��Left=�6�   $	��102��6�   �� 5   
   -   5        $ trackPosistion��5      $��
10,10,10,0q�6)   $	��Top=�6=   $ 5Q�6U   	 �� TickFrequency$	 10000Q�6�    . 6s  �� 5   
   -   5        
 ��Name$
 
nowPlaying��5      $
��Left=�6(   $��	10,42,0,0q�6C   $
��Wrap=�6V   $��Now playing��6j   $	��Top=�6}   $	��315��6�   �� 5   
   -	   5        $
 secondsText��5      $��436,42,10,0q�6)   $
��Wrap=�6>   $��	0:00/0:00��6R   $	��Top=�6c    �� TextAlignment$ Right=�6{   5      5      