function [] = convert_envmap_to_unity(filename)
e = EnvironmentMap(filename);
ecube = e.convertTo('cube');

%% Tonemapping
ec = 1 * ecube.data ./ (1 + ecube.data);
ec = ec .^ (1/1.2);

[height, width, depth] = size(ec);
pmin = prctile(ec(:), 3);
pmax = prctile(ec(:), 99.6);

ec = (ec - pmin);
ec = ec ./ (pmax - pmin);
ec = max(0, min(1, ec));
ec = uint8(ec*255);

imwrite(ec(1:round(height/4),round(width/3):round(2*width/3),:), 'forward.png');
imwrite(ec(round(3*height/4):end,round(width/3):round(2*width/3),:), 'backward.png');
imwrite(ec(round(height/4):round(2*height/4),1:round(width/3),:), 'left.png');
imwrite(ec(round(height/4):round(2*height/4),round(2*width/3):end,:), 'right.png');
imwrite(ec(round(3*height/4):end,round(width/3):round(2*width/3),:), 'top.png');
imwrite(ec(round(height/4):round(2*height/4),round(width/3):round(2*width/3),:), 'down.png');

end