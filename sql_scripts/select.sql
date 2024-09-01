select * from Tournament t, [Group] g, Game ga, PlayerGroup pg, Player p
where g.Tournament_id = t.id
  and ga.group_id = g.id
  and pg.group_id = g.id
  and pg.player_id = p.id
  and t.id = 2
  and p.Firstname = "Bob"
  and p.Lastname = "Brown";
  
 select * from Tournament;
  
 commit;